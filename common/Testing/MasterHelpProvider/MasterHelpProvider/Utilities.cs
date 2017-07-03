//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Word = Microsoft.Office.Interop.Word;
//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using WatiN.Core.DialogHandlers;


//namespace TestMasterHelpProvider
//{
//   public class Utilities
//   {

//       public Word.ApplicationClass _word = null;

//       //public void IsBrowserOpen()
//       //{
//       //    if (_ieBrowser != null)
//       //    {
//       //        _ieBrowser = new IE("http://www.randftesting.com/Home.aspx");
//       //    }
//       //}

//       public void CaptureScreenShoot()
//       {
//           object oMissing = Type.Missing;
//           object oTrue = true;

//           try
//           {
//               //instantiate a Word class and open it
//               if (_word == null)
//               {
//                   _word = new Word.ApplicationClass();
//                   _word.Visible = true;
//                   _word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oTrue);

//               }
//               //create a tempt file to hold the image
//               string fileName = "C:\\TestImage\\CaptureRFImages.jpg";
//               if (!File.Exists(fileName))
//               {
//                   CreateDir();

//                   FileInfo newFile = new FileInfo(@"c:\TestImage\CaptureRFImages.jpg");
//                   FileStream fs = newFile.Create();
//                   fs.Close();

//                   //string message = "File to save an image is not found.";
//                   //Assert.Fail(message);
//               }
//               //capture a screen shoot with WatiN
//               _ieBrowser.CaptureWebPageToFile("C:\\TestImage\\CaptureRFImages.jpg");
//               _word.ActiveWindow.Selection.Range.InlineShapes.AddPicture(fileName, ref oMissing, ref oMissing, ref oMissing);
//           }
//           catch (Exception ex)
//           {
//               throw ex;
//           }

//       }

//       public void CreateDir()
//       {
//           DirectoryInfo dir = new DirectoryInfo(@"C:\TestImage");
//           dir.Create();
//       }

//       public bool AssertPageContainsText(string expectedString)
//       {
//           try
//           {
//               Assert.IsTrue(_ieBrowser.ContainsText(expectedString));
//               return true;
//           }
//           catch (Exception)
//           {

//               Assert.IsFalse(_ieBrowser.ContainsText(expectedString), "Expected message was not present on page.");
//               return false;
//           }
//       }

//       public bool CheckPageContainsText(string expectedString)
//       {
//           if (_ieBrowser.ContainsText(expectedString))
//           {
//               return true;
//           }
//           else
//           {
//               return false;
//           }
//       }

//       public void CloseJSDialog()
//       {
//           ConfirmDialogHandler confirmDialog = new ConfirmDialogHandler();
//           Wait(10000);
//           _ieBrowser.AddDialogHandler(confirmDialog);
//           Wait(10000);
//           confirmDialog.OKButton.Click();
//       }

//       public void Wait(int milliseconds)
//       {
//           System.Threading.Thread.Sleep(milliseconds);
//       }

//       public void ClickBackBrowserButton()
//       {
//           _ieBrowser.Back();
//       }

//       public void CloseBrowser()
//       {
//           _ieBrowser.Close();
//       }


//   }
//}
