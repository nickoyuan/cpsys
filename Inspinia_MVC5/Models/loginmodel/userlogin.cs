// Decompiled with JetBrains decompiler
// Type: newtest.Models.loginmodel.userlogin
// Assembly: newtest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 461946BA-2AB1-41EE-A219-D965F6459033
// Assembly location: D:\rmit_work\Visual Studio 2015 Work\elementtest\site\site\wwwroot\bin\newtest.dll

using System.ComponentModel.DataAnnotations;

namespace newtest.Models.loginmodel
{
    public class userlogin
    {
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Username")]
        public string username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Password")]
        public string password { get; set; }
    }
}
