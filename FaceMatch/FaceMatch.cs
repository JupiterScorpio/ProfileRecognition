using Python.Runtime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace FaceMatch
{
    public class FaceCompare
    {
        private static bool bInit = false;
        private static IntPtr m_threadState;
        public static bool Initialize()
        {
            if (bInit)
                return bInit;

            string pythonDll = @"C:\Users\LUCKY\AppData\Local\Programs\Python\Python37\python37.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

            string pythonPath = @"C:\Users\LUCKY\anaconda3\envs\py37";
            Environment.SetEnvironmentVariable("PATH", $@"{pythonPath};" + Environment.GetEnvironmentVariable("PATH"));
            Environment.SetEnvironmentVariable("PYTHONHOME", pythonPath, EnvironmentVariableTarget.Process);

            string libPath = @"D:\All_tasks\py_task\Facial Recoginition\Image-recognation\face_compare";
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pythonPath}\\Lib;{pythonPath}\\Lib\\site-packages;{pythonPath}\\DLLs;{libPath};", EnvironmentVariableTarget.Process);
            
            PythonEngine.PythonHome = Environment.GetEnvironmentVariable("PYTHONHOME", EnvironmentVariableTarget.Process);
            PythonEngine.PythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
            try
            {
                Console.WriteLine("start Initialize ---------");
                PythonEngine.Initialize(); 
                m_threadState =  PythonEngine.BeginAllowThreads();
                Console.WriteLine("end Initialize ---------");
                bInit = true;
            }
            catch(Exception ex)
            {
                string LoginErrorMessage = ex.Message;
                bInit = false;
                return false;
            }
            return true;
        }
        public static void Shutdown()
        {
            //PythonEngine.EndAllowThreads(m_threadState);
            //PythonEngine.Shutdown();
        }
        public static float Compare(string image1, string image2)
        {
            dynamic dist = 10.0f;
            
            using (Py.GIL())
            {
                dynamic compareModule = Py.Import("compare_faces");
                string embName1 = string.Concat(Path.GetFileNameWithoutExtension(image1), "emb");
                embName1 = Path.GetDirectoryName(image1) + "\\" + embName1;
                string embName2 = string.Concat(Path.GetFileNameWithoutExtension(image2), "emb");
                embName2 = Path.GetDirectoryName(image2) + "\\" + embName2;

                dist = compareModule.compare(image1, image2);
                
            }

            return (float)dist;
        }
        public static bool MakeImageEmbedding(string image)
        {
            bool ret = false;
            try
            {
                using (Py.GIL())
                {
                    string embName = string.Concat(Path.GetFileNameWithoutExtension(image), ".emb");
                    embName = Path.GetDirectoryName(image) + "\\" + embName;
                    if (File.Exists(embName))
                        File.Delete(embName);

                    Console.WriteLine("----- Emb Name : ---------  " + embName);

                    dynamic compareModule = Py.Import("emebedding_faces");
                    ret = compareModule.get_embedding(image, embName);
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}