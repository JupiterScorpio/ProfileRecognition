﻿using AdminApp.ViewModels;
using ClientApp.Views;
using Newtonsoft.Json;
using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace ClientApp.ViewModels
{
    public class ClientMatchedViewModel : BaseViewModel
    {
        public static string strselPerson;
        private long totalPageSize = 0;
        private int records = 0;
        public ClientMatchedViewModel()
        {
            GetMatchedPhotos();
            //GetSelMatchedPhotos();
        }
        Frame rootFrame = Window.Current.Content as Frame;
        #region Foregroud

        private SolidColorBrush _personColorOnType = new SolidColorBrush(Color.FromArgb(255, 110, 110, 111));
        public SolidColorBrush PersonColorOnType
        {
            get { return _personColorOnType; }
            set { _personColorOnType = value; OnPropertyChanged("PersonColorOnType"); }
        }
        #endregion
        #region Elements Properties
        private string _newPassword = "";
        private string _policeStation = StaticContext.PoliceStation;
        private string _notificationMessage = string.Empty;
        private int _pageNo = 1;
        private int _selpageNo = 1;
        private string _passwordStrength = "";

        public string PoliceStation
        {
            get { return _policeStation; }
            set
            { _policeStation = value; OnPropertyChanged("PoliceStation"); }
        }
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { _notificationMessage = value; OnPropertyChanged("NotificationMessage"); }
        }

        public int PageNo
        {
            get { return _pageNo; }
            set { _pageNo = value; OnPropertyChanged("PageNo"); }
        }
        public int SelPageNo
        {
            get { return _selpageNo; }
            set { _selpageNo = value; OnPropertyChanged("SelPageNo"); }
        }
        #endregion
        #region Visibility Properties

        private Visibility _logoutPopupGridVis = Visibility.Collapsed;
        private Visibility _updatePasswordProgressRingVis = Visibility.Collapsed;
        private Visibility _resetPopupGridVis = Visibility.Collapsed;
        private Visibility _updatePasswordGridVis = Visibility.Visible;
        private Visibility _notificationPopupGridVis = Visibility.Collapsed;
        private Visibility _noDataValidationVis = Visibility.Collapsed;
        private Visibility _listBoxVis = Visibility.Visible;
        private Visibility _paginationButtonVis = Visibility.Collapsed;
        private Visibility _loadingBackgroundGridVis = Visibility.Collapsed;
        private Visibility _newPasswordValidationMsg = Visibility.Collapsed;
        private Visibility _disabledBtnVis = Visibility.Collapsed;
        private Visibility _seldisabledBtnVis = Visibility.Collapsed;
        private Visibility _disabledbackBtnVis = Visibility.Collapsed;
        private Visibility _seldisabledbackBtnVis = Visibility.Collapsed;
        private Visibility _photoBtnProgressVis = Visibility.Collapsed;
        private Visibility _photoBtnProgressRingVis = Visibility.Collapsed;
        public Visibility PhotoBtnProgressRingVis
        {
            get { return _photoBtnProgressRingVis; }
            set
            {
                _photoBtnProgressRingVis = value;
                OnPropertyChanged("PhotoBtnProgressRingVis");
            }
        }
        public Visibility PhotoBtnProgressVis
        {
            get { return _photoBtnProgressVis; }
            set
            {
                _photoBtnProgressVis = value;
                OnPropertyChanged("PhotoBtnProgressVis");
            }
        }
        public Visibility DisabledBtnVis
        {
            get { return _disabledBtnVis; }
            set
            {
                _disabledBtnVis = value;
                OnPropertyChanged("DisabledBtnVis");
            }
        }
        public Visibility SelDisabledBtnVis
        {
            get { return _seldisabledBtnVis; }
            set
            {
                _seldisabledBtnVis = value;
                OnPropertyChanged("SelDisabledBtnVis");
            }
        }
        public Visibility DisabledbackBtnVis
        {
            get { return _disabledbackBtnVis; }
            set
            {
                _disabledbackBtnVis = value;
                OnPropertyChanged("DisabledbackBtnVis");
            }
        }
        public Visibility SelDisabledbackBtnVis
        {
            get { return _seldisabledbackBtnVis; }
            set
            {
                _seldisabledbackBtnVis = value;
                OnPropertyChanged("SelDisabledbackBtnVis");
            }
        }
        public Visibility NewPasswordValidationMsg
        {
            get { return _newPasswordValidationMsg; }
            set
            {
                _newPasswordValidationMsg = value;
                OnPropertyChanged("NewPasswordValidationMsg");
            }
        }
        public Visibility LoadingBackgroundGridVis
        {
            get { return _loadingBackgroundGridVis; }
            set
            {
                _loadingBackgroundGridVis = value;
                OnPropertyChanged("LoadingBackgroundGridVis");
            }
        }
        public Visibility PaginationButtonVis
        {
            get { return _paginationButtonVis; }
            set
            {
                _paginationButtonVis = value;
                OnPropertyChanged("PaginationButtonVis");
            }
        }

        public Visibility NoDataValidationVis
        {
            get { return _noDataValidationVis; }
            set
            {
                _noDataValidationVis = value;
                OnPropertyChanged("NoDataValidationVis");
            }
        }

        public Visibility ListBoxVis
        {
            get { return _listBoxVis; }
            set
            {
                _listBoxVis = value;
                OnPropertyChanged("ListBoxVis");
            }
        }
        public Visibility LogoutPopupGridVis
        {
            get { return _logoutPopupGridVis; }
            set
            {
                _logoutPopupGridVis = value;
                OnPropertyChanged("LogoutPopupGrid");
            }
        }

        public Visibility UpdatePasswordProgressRingVis
        {
            get { return _updatePasswordProgressRingVis; }
            set
            {
                _updatePasswordProgressRingVis = value;
                OnPropertyChanged("UpdatePasswordProgressRingVis");
            }
        }

        public Visibility ResetPopupGridVis
        {
            get { return _resetPopupGridVis; }
            set
            {
                _resetPopupGridVis = value;
                OnPropertyChanged("ResetPopupGridVis");
            }
        }
        public Visibility UpdatePasswordGridVis
        {
            get { return _updatePasswordGridVis; }
            set
            {
                _updatePasswordGridVis = value;
                OnPropertyChanged("UpdatePasswordGridVis");
            }
        }
        public Visibility NotificationPopupGridVis
        {
            get { return _notificationPopupGridVis; }
            set
            {
                _notificationPopupGridVis = value;
                OnPropertyChanged("NotificationPopupGridVis");
            }
        }
        public string PasswordStrength
        {
            get { return _passwordStrength; }
            set
            {
                _passwordStrength = value; OnPropertyChanged("PasswordStrength");
            }
        }


        public string NewPassword
        {
            get { return _newPassword; }
            set
            {

                var data = CheckingPasswordStrength(value);
                if (data == PasswordScore.Blank)
                    PasswordStrength = "Include a number (0-9)";
                if (data == PasswordScore.charactelength)
                    PasswordStrength = "Include at least 8 characters long";
                if (data == PasswordScore.VeryWeak)
                    PasswordStrength = "Include at least 1 upper case and lower case characters (A-Z)";
                if (data == PasswordScore.Weak)
                    PasswordStrength = "Include a number (0-9) and at least 1 special character";
                if (data == PasswordScore.Medium)
                    PasswordStrength = "";
                if (data == PasswordScore.Strong)
                    PasswordStrength = "Include at least 1 special character";
                if (data == PasswordScore.VeryStrong)
                    PasswordStrength = "";

                NewPasswordValidationMsg = Visibility.Visible;
                if (string.IsNullOrEmpty(value))
                {
                    PasswordStrength = "Required";
                }
                _newPassword = value; OnPropertyChanged("NewPassword");

                /* _newPassword = value; OnPropertyChanged("NewPassword");*/
            }
        }

        #endregion

        #region Commands
        public ICommand LogoutCommand
        {
            get { return new DelegateCommand(UserLogout); }
        }
        public ICommand SureCommand
        {
            get { return new DelegateCommand(SureLogout); }
        }
        public ICommand DontLogoutCommand
        {
            get { return new DelegateCommand(DontLogout); }
        }
        public ICommand UploadCommand
        {
            get { return new DelegateCommand(NavigateToUploadPage); }
        }
        public ICommand UnmatchedPhotosCommand
        {

            get { return new DelegateCommand(NavigateToUnmatchedPage); }
        }
        public ICommand MatchedPhotosCommand
        {

            get { return new DelegateCommand(NavigateToMatchedPage); }
        }
        public ICommand AllPhotosCommand
        {
            get { return new DelegateCommand(NavigateToAllPhotosPage); }
        }

        public ICommand UpdateUserPasswordCommand
        {
            get { return new DelegateCommand(ResetUserPassword); }
        }

        public ICommand PreviousCommand
        {
            get { return new DelegateCommand(PreviousButton); }
        }
        public ICommand NextCommand
        {
            get { return new DelegateCommand(NextButton); }
        }
        public ICommand SelPreviousCommand
        {
            get { return new DelegateCommand(SelPreviousButton); }
        }
        public ICommand SelNextCommand
        {
            get { return new DelegateCommand(SelNextButton); }
        }
        public ICommand CancelbtnCommand
        {
            get { return new DelegateCommand(CancelPopup); }
        }
        #endregion
        #region List

        private List<MatchedPhotosResponseModel> _matchedPhotosList;
        private List<MatchedPhotosResponseModel> _SelmatchedPhotosList;
        private List<string> _matchedpersonsList;
        public List<MatchedPhotosResponseModel> MatchedPhotosList
        {
            get { return _matchedPhotosList; }
            set { _matchedPhotosList = value; OnPropertyChanged("MatchedPhotosList"); }
        }

        public List<MatchedPhotosResponseModel> SelMatchedPhotosList
        {
            get { return _SelmatchedPhotosList; }
            set { _SelmatchedPhotosList = value; OnPropertyChanged("SelMatchedPhotosList"); }
        }
        public List<string> MatchedPersonsList
        {
            get { return _matchedpersonsList; }
            set { _matchedpersonsList = value; OnPropertyChanged("MatchedPersonsList"); }
        }
        #endregion
        #region Popup Properties

        private bool _previousButtonEnable = false;
        private bool _nextButtonEnable = false;
        private bool _selpreviousButtonEnable = false;
        private bool _selnextButtonEnable = false;
        private bool _popupOpen = false;
        private bool _notificationPopup = false;
        private bool _logoutpopupOpen = false;

        public bool PreviousButtonEnable
        {
            get { return _previousButtonEnable; }
            set { _previousButtonEnable = value; OnPropertyChanged("PreviousButtonEnable"); }
        }
        public bool NextButtonEnable
        {
            get { return _nextButtonEnable; }
            set { _nextButtonEnable = value; OnPropertyChanged("NextButtonEnable"); }
        }
        public bool SelPreviousButtonEnable
        {
            get { return _selpreviousButtonEnable; }
            set { _selpreviousButtonEnable = value; OnPropertyChanged("SelPreviousButtonEnable"); }
        }
        public bool SelNextButtonEnable
        {
            get { return _selnextButtonEnable; }
            set { _selnextButtonEnable = value; OnPropertyChanged("SelNextButtonEnable"); }
        }
        public bool PopupOpen
        {
            get { return _popupOpen; }
            set { _popupOpen = value; OnPropertyChanged("PopupOpen"); }
        }
        public bool LogoutPopupOpen
        {
            get { return _logoutpopupOpen; }
            set { _logoutpopupOpen = value; OnPropertyChanged("LogoutPopupOpen"); }
        }
        public bool NotificationPopup
        {
            get { return _notificationPopup; }
            set { _notificationPopup = value; OnPropertyChanged("NotificationPopup"); }
        }
        #endregion
        #region Functions

        private PasswordScore CheckingPasswordStrength(string password)
        {
            int score = 1;
            if (password.Length < 8)
                return PasswordScore.charactelength;
            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length == 8)
                score++;
            if (password.Length > 12)
                score++;
            if (password.Length < 12)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))   //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)) //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;
            return (PasswordScore)score;
        }



        private void CancelPopup()
        {
            LoadingBackgroundGridVis = Visibility.Collapsed;
            ResetPopupGridVis = Visibility.Visible;
            PopupOpen = false;
        }
        private async void PreviousButton()
        {
            ResponseBody response = new ResponseBody();
            ClientPagination page = new ClientPagination();
            NextButtonEnable = true;
            PageNo = PageNo - 1;
            records = records - page.PageSize;
            page = new ClientPagination()
            {
                PageNum = PageNo,
            };

            if (PageNo <= 1)
            {
                DisabledbackBtnVis = Visibility.Visible;
                PreviousButtonEnable = false;
            }


            if (PageNo >= 1)
            {
                DisabledBtnVis = Visibility.Collapsed;
                response = await StaticContext.GetMatchApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    List<MatchedPhotosResponseModel> list = new List<MatchedPhotosResponseModel>();
                    list = JsonConvert.DeserializeObject<List<MatchedPhotosResponseModel>>(response.Data);
                    MatchedPhotosList = list;
                }
                else
                {
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
            }
        }
        private async void SelPreviousButton()
        {
            ResponseBody response = new ResponseBody();
            ClientPagination page = new ClientPagination();
            SelNextButtonEnable = true;
            SelPageNo = SelPageNo - 1;
            records = records - page.PageSize;
            page = new ClientPagination()
            {
                PageNum = SelPageNo,
            };

            if (SelPageNo <= 1)
            {
                SelDisabledbackBtnVis = Visibility.Visible;
                SelPreviousButtonEnable = false;
            }


            if (SelPageNo >= 1)
            {
                SelDisabledBtnVis = Visibility.Collapsed;
                response = await StaticContext.GetSelectMatchApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    List<MatchedPhotosResponseModel> list = new List<MatchedPhotosResponseModel>();
                    list = JsonConvert.DeserializeObject<List<MatchedPhotosResponseModel>>(response.Data);
                    SelMatchedPhotosList = list;
                }
                else
                {
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
            }
        }
        private async void NextButton()
        {
            ResponseBody response = new ResponseBody();
            ClientPagination page = new ClientPagination();
            if (totalPageSize > records)
            {
                PageNo = PageNo + 1;
                page = new ClientPagination()
                {
                    PageNum = PageNo,
                };

                records = PageNo * page.PageSize;

                if (records >= totalPageSize)
                {
                    DisabledBtnVis = Visibility.Visible;
                    NextButtonEnable = false;
                }


                PreviousButtonEnable = true;
                response = await StaticContext.GetMatchApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    DisabledbackBtnVis = Visibility.Collapsed;
                    List<MatchedPhotosResponseModel> list = new List<MatchedPhotosResponseModel>();
                    list = JsonConvert.DeserializeObject<List<MatchedPhotosResponseModel>>(response.Data);
                    MatchedPhotosList = list;
                }
                else
                {
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
            }
        }
        private async void SelNextButton()
        {
            ResponseBody response = new ResponseBody();
            ClientPagination page = new ClientPagination();
            if (totalPageSize > records)
            {
                SelPageNo = SelPageNo + 1;
                page = new ClientPagination()
                {
                    PageNum = SelPageNo,
                };

                records = SelPageNo * page.PageSize;

                if (records >= totalPageSize)
                {
                    SelDisabledBtnVis = Visibility.Visible;
                    SelNextButtonEnable = false;
                }


                SelPreviousButtonEnable = true;
                response = await StaticContext.GetSelectMatchApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    SelDisabledbackBtnVis = Visibility.Collapsed;
                    List<MatchedPhotosResponseModel> list = new List<MatchedPhotosResponseModel>();
                    list = JsonConvert.DeserializeObject<List<MatchedPhotosResponseModel>>(response.Data);
                    SelMatchedPhotosList = list;
                }
                else
                {
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
            }
        }

        private async void ResetUserPassword()
        {
            try
            {
                ResetPassword resetpassword = new ResetPassword
                {
                    UserPassword = NewPassword,
                    Code = StaticContext.UserId
                };

                if (resetpassword.UserPassword.Length >= 8 && !string.IsNullOrEmpty(resetpassword.Code) && !string.IsNullOrEmpty(resetpassword.UserPassword))
                {
                    ClientPasswordVis();
                    _ = Task.Run(async () =>
                    {
                        ResponseBody response = new ResponseBody();
                        response = await StaticContext.ResetPasswordApiUrl.PostAsync<ResponseBody>(resetpassword);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            if (response.IsValidated)
                            {
                                NewPassword = "";
                                CancelPopup();
                                OpenNotificationPopup();
                                NotificationMessage = response.Detail;
                                CloseMessageTimer();
                                UpdatePasswordGridVis = Visibility.Visible;
                                UpdatePasswordProgressRingVis = Visibility.Collapsed;
                            }
                            else
                            {
                                OpenNotificationPopup();
                                NotificationMessage = response.Message;
                                CloseMessageTimer();
                            }
                        }).AsTask();
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(resetpassword.UserPassword))
                    {
                        NewPasswordValidationMsg = Visibility.Visible;
                        PasswordStrength = "Required";
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SureLogout()
        {
            LogoutPopupGridVis = Visibility.Collapsed;
            LogoutPopupOpen = false;
            Logout();
        }

        private void DontLogout()
        {
            LogoutPopupGridVis = Visibility.Collapsed;
            LogoutPopupOpen = false;
        }
        private void UserLogout()
        {
            LogoutPopupGridVis = Visibility.Visible;
            LogoutPopupOpen = true;

        }
        private void Logout()
        {
            if (StaticContext.UserId != "")
            {
                Frame rootFrame = Window.Current.Content as Frame;

                LoginModel model = new LoginModel()
                {
                    AccessToken = "",
                    UserId = "",
                };

                RealmDb<LoginModel>.Update(model, d => d.UserId == StaticContext.UserId);
                RealmDb<LoginModel>.Delete(d => d.UserId == StaticContext.UserId);
                var userData1 = RealmDb<LoginModel>.Get(d => d.UserId == StaticContext.UserId);

                StaticContext.UserId = "";
                rootFrame.Navigate(typeof(ClientLoginPage));
            }
        }
        public void NavigateToUploadPage()
        {
            rootFrame.Navigate(typeof(ClientDashboard));
        }
        public void NavigateToUnmatchedPage()
        {
            rootFrame.Navigate(typeof(ClientUnmatchedPhotos));
        }
        public void NavigateToMatchedPage()
        {
            rootFrame.Navigate(typeof(ClientMatchedPhotos));
        }
        public void NavigateToAllPhotosPage()
        {
            rootFrame.Navigate(typeof(ClientAllPhotos));
        }

        private async void GetMatchedPhotos()
        {
            ClientPagination page = new ClientPagination();
            try
            {
                PhotoBtnProgressVis = Visibility.Collapsed;
                PhotoBtnProgressRingVis = Visibility.Visible;
                _ = Task.Run(async () =>
                {
                    ResponseBody response = new ResponseBody();
                    response = await StaticContext.GetMatchApiUrl.PostAsync<ResponseBody>(page);
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        if (response.IsValidated)
                        {
                            if (response.Data != null)
                            {
                                ListBoxVis = Visibility.Visible;
                                List<MatchedPhotosResponseModel> list = new List<MatchedPhotosResponseModel>();
                                list = JsonConvert.DeserializeObject<List<MatchedPhotosResponseModel>>(response.Data);
                                List<string> strlst=new List<string>();
                                foreach(MatchedPhotosResponseModel item in list)
                                {
                                    string stritem = item.Name;
                                    strlst.Add(stritem);
                                }
                                MatchedPersonsList = strlst;
                                MatchedPhotosList = list;
                                PersonColorOnType = new SolidColorBrush(Colors.AliceBlue);
                                PreviousButtonEnable = false;
                                PhotoBtnVis();
                                if (response.Total > page.PageSize)
                                {
                                    totalPageSize = response.Total;
                                    NextButtonEnable = true;
                                    DisabledbackBtnVis = Visibility.Visible;
                                    DisabledBtnVis = Visibility.Collapsed;
                                }
                                else
                                {
                                    NextButtonEnable = false;
                                    DisabledBtnVis = Visibility.Visible;
                                    DisabledbackBtnVis = Visibility.Visible;
                                }
                            }
                            else
                            {
                                ListBoxVis = Visibility.Collapsed;
                                NoDataValidationVis = Visibility.Visible;
                                PhotoBtnVis();
                            }
                        }
                        else
                        {
                            OpenNotificationPopup();
                            PhotoBtnProgressVis = Visibility.Visible;
                            PhotoBtnProgressRingVis = Visibility.Collapsed;
                            NotificationMessage = response.Message;
                            CloseMessageTimer();
                        }
                    }).AsTask();
                });
            }
            catch (Exception)
            {

                return;
            }
        }

        public async Task GetSelMatchedPhotos(string imgURL)
        {
            ClientPagination page = new ClientPagination();
            try
            {
                //string apiURL = StaticContext.ApiUrl;
                //int index = imgURL.IndexOf(apiURL);
                //int length = apiURL.Length;
                //imgURL = imgURL.Substring(index + length);
                //string qimgURL = imgURL.Replace("\\\\", "\\");
                int index = imgURL.LastIndexOf('/');
                string imgName = imgURL.Substring(index + 1);

                PhotoBtnProgressVis = Visibility.Collapsed;
                PhotoBtnProgressRingVis = Visibility.Visible;
                _ = Task.Run(async () =>
                {
                    ResponseBody response = new ResponseBody();
                    var api = StaticContext.GetSelectMatchApiUrl + imgName;
                    response = await api.PostAsync<ResponseBody>(page);
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        if (response.IsValidated)
                        {
                            if (response.Data != null)
                            {
                                ListBoxVis = Visibility.Visible;
                                List<MatchedPhotosResponseModel> list = new List<MatchedPhotosResponseModel>();
                                list = JsonConvert.DeserializeObject<List<MatchedPhotosResponseModel>>(response.Data);

                                SelMatchedPhotosList = list;
                                PersonColorOnType = new SolidColorBrush(Colors.AliceBlue);
                                SelPreviousButtonEnable = false;
                                PhotoBtnVis();
                                if (response.Total > page.PageSize)
                                {
                                    totalPageSize = response.Total;
                                    SelNextButtonEnable = true;
                                    SelDisabledbackBtnVis = Visibility.Visible;
                                    SelDisabledBtnVis = Visibility.Collapsed;
                                }
                                else
                                {
                                    SelNextButtonEnable = false;
                                    SelDisabledBtnVis = Visibility.Visible;
                                    SelDisabledbackBtnVis = Visibility.Visible;
                                }
                            }
                            else
                            {
                                ListBoxVis = Visibility.Collapsed;
                                NoDataValidationVis = Visibility.Visible;
                                PhotoBtnVis();
                            }
                        }
                        else
                        {
                            OpenNotificationPopup();
                            PhotoBtnProgressVis = Visibility.Visible;
                            PhotoBtnProgressRingVis = Visibility.Collapsed;
                            NotificationMessage = response.Message;
                            CloseMessageTimer();
                        }
                    }).AsTask();
                });
            }
            catch (Exception)
            {
                return;
            }
        }
        
        public void CloseMessageTimer()
        {
            DispatcherTimer responseTimer = new DispatcherTimer();
            responseTimer.Tick += (senders, args) =>
            {
                CollapseNotificationPopup();
                responseTimer.Stop();
            };
            responseTimer.Interval = TimeSpan.FromSeconds(3);
            responseTimer.Start();
        }

        public void OpenNotificationPopup()
        {
            NotificationPopupGridVis = Visibility.Visible;
            NotificationPopup = true;
        }
        public void CollapseNotificationPopup()
        {
            NotificationPopupGridVis = Visibility.Collapsed;
            NotificationMessage = string.Empty;
            NotificationPopup = false;
        }

        public void ClientPasswordVis()
        {
            UpdatePasswordGridVis = Visibility.Collapsed;
            UpdatePasswordProgressRingVis = Visibility.Visible;
        }
        public void PhotoBtnVis()
        {
            PhotoBtnProgressVis = Visibility.Visible;
            PhotoBtnProgressRingVis = Visibility.Collapsed;
        }
        #endregion
    }
}
