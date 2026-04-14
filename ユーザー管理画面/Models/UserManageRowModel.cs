using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ユーザー管理画面.Models
{
    public class UserManageRowModel
    {
        public string ユーザーID { get; set; }
        public string 氏名 { get; set; }
        public string 性別 { get; set; }
        public string 生年月日 { get; set; }
        public string 電話番号 { get; set; }
        public string メール { get; set; }
        public string 住所 { get; set; }
        public string 所属 { get; set; }
        public string 役職 { get; set; }
    }
}