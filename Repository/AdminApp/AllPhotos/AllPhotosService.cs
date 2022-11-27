using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;

namespace Repository
{

    public class AllPhotosService : IAllPhotos
    {
        private Context db;
        private float dist = 1.0f;
        
        
        public AllPhotosService(Context _db)
        {
            this.db = _db;
        }

        public async Task<Response> FetchAllPhotos(Pagination pages)
        {
            Response response = new Response();
            var userData = await db.LoadStoredProcedure("[dbo].[FetchProfile]")
                                   .WithSqlParams(("PageSize", pages.PageSize),
                                                  ("PageNum", pages.PageNum),
                                                  ("AspNetUserId", pages.AspNetUserId.Decode()))
                                  .ExecuteStoredProcedureAsync<AllPhotosResponseModel>();
            if (!userData.Any())
                throw new ApplicationException(Message.ErrorMessage);

          
            response.Data = userData.Serialize();
            response.Total = userData[0].Total;

            return response;
        }

        public async Task<Response> DeleteProfilePhotos(string Code)
        {
            Response response = new Response();
            Infrastructure.Profile profile = db.Profiles.FirstOrDefault(x => x.Id == Code.Decode().ToInt());
            if (profile is null)
                throw new ApplicationException(Message.ErrorMessage);

            profile.IsDelete = true;
            response.Detail = Message.DeletePhotoSuccess;
            db.Update(profile);
            await db.SaveChangesAsync();
            return response;
        }

        public async Task<Response> FetchMatchPhotos(Pagination pages)
        {
            
            var UserId = new SqlParameter("@UserId", pages.AspNetUserId.Decode());
           
            var allData = await db.LoadStoredProcedure("[dbo].[FetchAllProfile]")
                                  .WithSqlParams(("AspNetUserId", pages.AspNetUserId.Decode()))
                                 .ExecuteStoredProcedureAsync<AllPhotosResponseModel>();
           
            if (!allData.Any())
                throw new ApplicationException(Message.ErrorMessage);                      

            Response response = new Response();

            var objList = new List<AllPhotosResponseModel>();           

            List<Infrastructure.Profile> Profiles = await db
                                                        .Profiles
                                                        .FromSqlRaw("SELECT * FROM [dbo].[Profiles] WHERE AspNetUserId <> @UserId and IsDelete = 0", UserId)
                                                        .ToListAsync();
            bool bInit = FaceMatch.FaceCompare.Initialize();
            if (!bInit)
                throw new ApplicationException(Message.ErrorMessage);

            for (int k = 0; k < allData.Count; ++k)
            {               
                bool isLost = allData[k].IsLost.Equals("Found Person");
                string embName = allData[k].ImageURL.Replace("jpg", "emb");
                embName = Path.Combine(Static.WebRootPath, embName);
                if (!File.Exists(embName))
                {
                    bool ret = FaceMatch.FaceCompare.MakeImageEmbedding(Path.Combine(Static.WebRootPath, allData[k].ImageURL));
                    if (!ret) 
                        continue;
                }

                for (int i = 0; i < Profiles.Count; ++i)
                {
                    if( isLost == Profiles[i].IsLost && allData[k].AspNetUserId == Profiles[i].AspNetUserId)
                        continue ;
                                       
                    string emb = Profiles[i].ImageURL.Replace("jpg", "emb");
                    emb = Path.Combine(Static.WebRootPath, emb);
                    if (!File.Exists(emb))
                    {
                        bool ret = FaceMatch.FaceCompare.MakeImageEmbedding(Path.Combine(Static.WebRootPath, Profiles[i].ImageURL));
                        if (!ret) continue;
                    }

                    float score = FaceMatch.FaceCompare.Compare(embName, emb);
                    if (score < dist)
                    {
                        AllPhotosResponseModel obj = Activator.CreateInstance<AllPhotosResponseModel>();
                        obj = allData[k];                      
                        obj.RowNo = objList.Count+1;
                        objList.Add(obj);
                        break;
                    }
                }
            }
                       
            response.Data = objList.Serialize();
            response.Total = objList.Count;

            FaceMatch.FaceCompare.Shutdown();
            return response;
        }

        public async Task<Response> FetchUnMatchPhotos(Pagination pages)
        {
            var UserId = new SqlParameter("@UserId", pages.AspNetUserId.Decode());          

            var allData = await db.LoadStoredProcedure("[dbo].[FetchAllProfile]")
                                  .WithSqlParams(("AspNetUserId", pages.AspNetUserId.Decode()))
                                 .ExecuteStoredProcedureAsync<AllPhotosResponseModel>();

            if (!allData.Any())
                throw new ApplicationException(Message.ErrorMessage);


            Response response = new Response();
            var objList = new List<AllPhotosResponseModel>();

            List<Infrastructure.Profile> Profiles = await db
                                                        .Profiles
                                                        .FromSqlRaw("SELECT * FROM [dbo].[Profiles] WHERE AspNetUserId <> @UserId and IsDelete = 0", UserId)
                                                        .ToListAsync();
            bool bInit = FaceMatch.FaceCompare.Initialize();
           
            for (int k = 0; k < allData.Count; ++k)
            {
                bool isLost = allData[k].IsLost.Equals("Found Person");
                string embName = allData[k].ImageURL.Replace("jpg", "emb");
                embName = Path.Combine(Static.WebRootPath, embName);
                if (!File.Exists(embName))
                {
                    bool ret = FaceMatch.FaceCompare.MakeImageEmbedding(Path.Combine(Static.WebRootPath, allData[k].ImageURL));
                    if (!ret) 
                        continue;
                }
                int i = 0;
                for (; i < Profiles.Count; ++i)
                {
                    if (isLost == Profiles[i].IsLost && allData[k].AspNetUserId == Profiles[i].AspNetUserId)
                        continue;

                    string emb = Profiles[i].ImageURL.Replace("jpg", "emb");
                    emb = Path.Combine(Static.WebRootPath, emb);
                    if (!File.Exists(emb))
                    {
                        bool ret = FaceMatch.FaceCompare.MakeImageEmbedding(Path.Combine(Static.WebRootPath, Profiles[i].ImageURL));
                        if (!ret) continue;
                    }

                    float score = FaceMatch.FaceCompare.Compare(embName, emb);
                    if (score < dist)
                    {                       
                        break;
                    }
                }

                if (i == Profiles.Count)
                {
                    AllPhotosResponseModel obj = Activator.CreateInstance<AllPhotosResponseModel>();
                    obj = allData[k];
                    obj.RowNo = objList.Count+1;
                    objList.Add(obj);
                }
            }

            if (objList.Count > 0)
            {
                response.Data = objList.Serialize();
                response.Total = objList.Count;
            }

            if (bInit) 
                FaceMatch.FaceCompare.Shutdown();

            return response;
        }

        public async Task<Response> FetchSelectPhotos(string imgURL)
        {
            Response response = new Response();
            Infrastructure.Profile profile = db.Profiles.FirstOrDefault(x => x.ImageURL == imgURL);
            if (profile is null)
                throw new ApplicationException(Message.ErrorMessage);
                       
            var objList = new List<AllPhotosResponseModel>();

            var allData = await db.LoadStoredProcedure("[dbo].[FetchSelectProfile]")
                                   .WithSqlParams(("AspNetUserId", profile.AspNetUserId))
                                  .ExecuteStoredProcedureAsync<AllPhotosResponseModel>();
            if (!allData.Any())
                throw new ApplicationException(Message.ErrorMessage);           

            {
                bool bInit = FaceMatch.FaceCompare.Initialize();
               
                bool isLost = profile.IsLost;
                string embName = profile.ImageURL.Replace("jpg", "emb");
                embName = Path.Combine(Static.WebRootPath, embName);
                if (!File.Exists(embName))
                {
                    //throw new ApplicationException("No Face prop data of this image");
                    return response;

                    //bool ret = FaceMatch.FaceCompare.MakeImageEmbedding(Path.Combine(Static.WebRootPath, profile.ImageURL));
                    //if (!ret)
                    //    return response;
                }     

                for (int i = 0; i < allData.Count; ++i)
                {                   
                    string emb = allData[i].ImageURL.Replace("jpg", "emb");
                    emb = Path.Combine(Static.WebRootPath, emb);
                    if (!File.Exists(emb))
                    {
                        continue;
                        //bool ret = FaceMatch.FaceCompare.MakeImageEmbedding(Path.Combine(Static.WebRootPath, allData[i].ImageURL));
                        //if (!ret) continue;
                    }

                    float score = FaceMatch.FaceCompare.Compare(embName, emb);
                    if (score < dist)
                    {
                        AllPhotosResponseModel obj = Activator.CreateInstance<AllPhotosResponseModel>();
                        obj.Id = allData[i].Id;
                        obj.Name = allData[i].Name;
                        obj.CPName = allData[i].CPName;
                        obj.CPPhoneNo = allData[i].CPPhoneNo;
                        obj.GDCaseNo = allData[i].GDCaseNo;
                        obj.OfficerName = allData[i].OfficerName;
                        obj.OfficerBPNo = allData[i].OfficerBPNo;
                        obj.OfficerPhoneNo = allData[i].OfficerPhoneNo;
                        obj.Remarks = allData[i].Remarks;
                        obj.ImageURL = allData[i].ImageURL;
                        obj.IsLost = allData[i].IsLost;
                        obj.CreatedDate = allData[i].CreatedDate; 
                        obj.PoliceStation = allData[i].PoliceStation;
                        obj.RowNo = objList.Count+1;
                        objList.Add(obj);                        
                    }
                }

                if (bInit)
                    FaceMatch.FaceCompare.Shutdown();
            }
           

            response.Data = objList.Serialize();
            response.Total = objList.Count;

            return response;
        }
    }
}
