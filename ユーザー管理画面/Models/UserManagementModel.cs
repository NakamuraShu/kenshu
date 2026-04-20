using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ユーザー管理画面.Models
{
    public class UserManagementModel
    {
        public List<UserManageRowModel> 検索結果;
        public List<UserManageRowModel> 表示一覧;
        public List<string> 選択一覧 { get; set; }
        public int 表示件数;
        public int 現ページ;
        public string ソート列;
        public string ソート順;
        public string 表示区分 { get; set; }

        public void List(UserManageConditionModel condition)
        {
            現ページ = 1;

            string sql = "select *, cast(TIMESTAMPDIFF(YEAR, str_to_date(p.BIRTHDAY,'%Y%m%d'), CURDATE()) as char) as YEAR_OLD "
                    + "from user_master u inner join profile_info p on u.USER_ID = p.USER_ID ";

            表示区分 = condition.表示区分;
            if (!string.IsNullOrEmpty(表示区分))
            {
                sql = sql + "where u.TYPE='" + condition.表示区分 + "'";
            }

            MySqlConnection con = new MySqlConnection(
                "server=localhost;port=3306;userid=csharp;password=csharp;" +
                "database = csharp; convert zero datetime=True");

                con.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(sql, con);

                DataTable tb = new DataTable();

                da.Fill(tb);
                da.Dispose();
                con.Close();

                検索結果 = new List<UserManageRowModel>();
                foreach (DataRow row in tb.Rows)
                {
                    UserManageRowModel result = new UserManageRowModel();
                result.ユーザーID = row["USER_ID"].ToString();
                result.氏名 = row["USER_NAME"].ToString();
                result.性別 = row["SEX"].ToString();
                result.生年月日 = row["BIRTHDAY"].ToString();
                result.電話番号 = row["TEL"].ToString();
                result.メール = row["EMAIL"].ToString();
                result.住所 = row["ADDRESS"].ToString();
                result.所属 = row["POSITION"].ToString(); 
                result.役職 = row["AFFILIATION"].ToString();

                検索結果.Add(result);
            }
        }


        public void GetPage(int rowCount, int pageNum)
        {
            if (rowCount == 0 || 検索結果.Count <= rowCount)
            {
                表示一覧 = 検索結果;
            }
            else
            {
                if (rowCount == 表示件数)
                {
                    表示一覧 = 検索結果
                        .Where((item, index) => index >= (pageNum - 1) * rowCount && index < pageNum * rowCount)
                        .ToList();
                }
                else
                {
                    表示一覧 = 検索結果.Where((item, index) => index < rowCount).ToList();
                }
            }
            表示件数 = rowCount;
            現ページ = pageNum;
        }

        public void SortAll(string colName, string sortOrder)
        {
            if (sortOrder == "▲" || sortOrder == "")
            {
                if (ソート列 == "氏名") 検索結果 = 検索結果.OrderBy(x => x.氏名).ToList();
                if (ソート列 == "性別") 検索結果 = 検索結果.OrderBy(x => x.性別).ToList();
                if (ソート列 == "生年月日") 検索結果 = 検索結果.OrderBy(x => x.生年月日).ToList();
                if (ソート列 == "電話番号") 検索結果 = 検索結果.OrderBy(x => x.電話番号).ToList();
                if (ソート列 == "メール") 検索結果 = 検索結果.OrderBy(x => x.メール).ToList();
                if (ソート列 == "所属") 検索結果 = 検索結果.OrderBy(x => x.所属).ToList();
                if (ソート列 == "役職") 検索結果 = 検索結果.OrderBy(x => x.役職).ToList();
            }
            else
            {
                if (ソート列 == "氏名") 検索結果 = 検索結果.OrderByDescending(x => x.氏名).ToList();
                if (ソート列 == "性別") 検索結果 = 検索結果.OrderByDescending(x => x.性別).ToList();
                if (ソート列 == "生年月日") 検索結果 = 検索結果.OrderByDescending(x => x.生年月日).ToList();
                if (ソート列 == "電話番号") 検索結果 = 検索結果.OrderByDescending(x => x.電話番号).ToList();
                if (ソート列 == "メール") 検索結果 = 検索結果.OrderByDescending(x => x.メール).ToList();
                if (ソート列 == "所属") 検索結果 = 検索結果.OrderByDescending(x => x.所属).ToList();
                if (ソート列 == "役職") 検索結果 = 検索結果.OrderByDescending(x => x.役職).ToList();
            }
        }

        public void Sort(string colName, string sortOrder)
        {
            ソート列 = colName;
            ソート順 = sortOrder;
            現ページ = 1;
            SortAll(colName, sortOrder);
            GetPage(表示件数, 現ページ);
        }
        public void DeleteUser(UserManageConditionModel condition)
        {
            string str = "";
            bool check = false;
            int i = 1;

            foreach (string id in 選択一覧)
            {
                if (選択一覧.Count > 1)
                {
                    if (!check)
                    {
                        check = true;
                        str += " in (";
                    }

                    if (選択一覧.Count > i)
                    {
                        str += " '" + id + "' ,";
                    }
                    else
                    {
                        str += "'" + id + "' )";
                    }
                    i++;
                }
                else
                {
                    str += "= '" + id + "' ";
                }
            }

            string sql = "update user_master set type = 2 where user_id " + str;

            MySqlConnection con = new MySqlConnection(
                "server=localhost;port=3306;userid=csharp;password=csharp;" +
                "database = csharp; convert zero datetime=True");

            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery(); 
            con.Close();

            List(condition);

            SortAll(ソート列, ソート順);
            GetPage(表示件数, 現ページ);
        }

        public void InsertUser(UserManageConditionModel condition)
        {
            string str = "";
            bool check = false;
            int i = 1;

            foreach (string id in 選択一覧)
            {
                if (選択一覧.Count > 1)
                {
                    if (!check)
                    {
                        check = true;
                        str += " in (";
                    }

                    if (選択一覧.Count > i)
                    {
                        str += " '" + id + "' ,";
                    }
                    else 
                    { 
                        str += "'" + id + "' )";
                        
                    }
                    i++;
                }
                else 
                { 
                    str += "= '" + id + "' "; 
                }
            }

            string sql = "update user_master set type = 2 where user_id " + str;

            MySqlConnection con = new MySqlConnection(
                "server=localhost;port=3306;userid=csharp;password=csharp;" +
                "database = csharp; convert zero datetime=True");

            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();

            List(condition);

            SortAll(ソート列, ソート順);
            GetPage(表示件数, 現ページ);
        }
    }
}