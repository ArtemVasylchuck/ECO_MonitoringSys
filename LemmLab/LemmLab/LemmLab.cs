using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LemmLab
{
    public class LemmManager
    {
       
        List<Regex> lemmListEnd;
        /// <summary>
        /// ���������� ������ � ���������� ����� �� ���� ����� ����������� � ���� ������.
        /// </summary>
        /// <param name="end">������, �� ��������������.</param>
        /// <returns>���������� �����.</returns>
        private Regex RegExEnd(string end)
        {
            return new Regex( end +"$");
        }
		private Regex RegExBegin(string begin)
		{
			return new Regex("^" + begin);
		}
		/// <summary>
		/// �����������.
		/// </summary>
		public LemmManager()
        {
            lemmListEnd = new List<Regex>();
            lemmListEnd.Add(RegExEnd("������"));
            lemmListEnd.Add(RegExEnd("������"));
            lemmListEnd.Add(RegExEnd("����"));
            lemmListEnd.Add(RegExEnd("�����"));
            lemmListEnd.Add(RegExEnd("�����"));
            lemmListEnd.Add(RegExEnd("����"));
            lemmListEnd.Add(RegExEnd("����"));
            lemmListEnd.Add(RegExEnd("����"));
            lemmListEnd.Add(RegExEnd("���"));
            lemmListEnd.Add(RegExEnd("���"));
            lemmListEnd.Add(RegExEnd("���"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("���"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("��"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
            lemmListEnd.Add(RegExEnd("�"));
			lemmListEnd.Add(RegExBegin("���"));
			lemmListEnd.Add(RegExBegin("���"));
			lemmListEnd.Add(RegExBegin("��"));
			lemmListEnd.Add(RegExBegin("��"));
		}
        /// <summary>
        /// �������� �����.
        /// </summary>
        /// <param name="str">�����, �� ������� �����������.</param>
        /// <returns>������������ �����.</returns>
        public string ToLemm(string str)
        {
            foreach( Regex re in lemmListEnd)
            {
                if (re.IsMatch(str))
                {
                    str = re.Replace(str, "");
                }
            }
            return str;
        }
        /// <summary>
        /// ���������� ������ � ����� ���.
        /// </summary>
        /// <param name="str">������, �� ������� ������������.</param>
        /// <returns>������ ���.</returns>
        public string[] ToWords(string str)
        {
            
            Regex re = new Regex("(\\s)|[.,:;\"\"()]");
            return re.Split(str.ToLower());
        }
        /// <summary>
        /// ���������� ���� html-������� � ����� ���. ���� html-���� ��� ����� ����������.
        /// </summary>
        /// <param name="str">���� html-�������.</param>
        /// <returns>����� ���.</returns>
        public string[] ToWordsHTML(string str)                                 //   (\\d)+\\/(?!\\d)|\\/(?!\\d) - 645/97   \\/+ - 645 97
        {
            Regex re1 = new Regex("(\\s)|[.,:;\"()]");
            Regex re2 = new Regex("<(.|\\s|&quot)*?>|[A-Za-z|!|[|=|\\|{|}|&|$|^|�|_|�|�|�|�|�|?|�|%|>+|<+|�]+|\\]|\\/+|\\-(?=\\d)|\\-(?=%)|\\-(?=\\s)|\\-$|\\-{2,5}"); 
            Regex re3 = new Regex("<script language(.|\\s)*?<\\/script>|<div class=\"footer_support col-lg-4 col-md-6 order-md-1\">(.|\\s)*?</div>|<div class=\"cc-block\">(.|\\s)*?</div>|<div class=\"kitsoft-block hidden\">(.|\\s)*?</div>|<ul class=\"column-3-list my-3\">(.|\\s)*?</ul>|<label for=\"link-only\">(.|\\s)*?</div>|<h4 class=\"social-title mb-3\">(.|\\s)*?</h4>|<nobr>(.|\\s)*?</div>|<small class=\"text-muted\">(.|\\s)*?</small>|<div class=\"modal-body\">(.|\\s)*?</div>|<h3 class=\"alert-heading danger alert-no-flexbox\">(.|\\s)*?</noscript>|<ul class=\"navbar-nav mr-auto\">(.|\\s)*?</ul>|<i class=\"icn-rewind\"(.|\\s)*?</a>|<span class=\"d-lg-none\">(.|\\s)*?</div>|<div class=\"checkbox mt-3\">(.|\\s)*?</div>|<div class=\"clearfix\">(.|\\s)*?</a>|target=\"mail_blank\">(.|\\s)*?</div>|<nav class=\"nav nav-separated btn toolbar flex-wrap\">(.|\\s)*?</nav>|<div class=\"mb-3\">(.|\\s)*?</div>|<label for=\"orfo-name\">(.|\\s)*?</label>|<label for=\"orfo-email\">(.|\\s)*?</label>|<div class=\"modal-footer\">(.|\\s)*?</div>|<label for=\"link-code\">(.|\\s)*?</label>|<script>(.|\\s)*?</script>|<div class=\"modal-content\">(.|\\s)*?</div>|<div class=\"nav\">(.|\\s)*?</nav>|<button class=\"btn btn-link mr-3\"(.|\\s)*?</button>|<label for=\"link-code2\">(.|\\s)*?</label>|<div class=\"center-cell\">(.|\\s)*?</div>|<style>(.|\\s)*?<\\/style>");
            Regex re4 = new Regex("'{2}");
           // Regex re5 = new Regex("\\/(?=\\d)");

            return (from a in re1.Split(re2.Replace(re3.Replace(re4.Replace(str.ToLower(), ""), ""), " ")) where a.Trim() != "" select a.Trim()).ToArray();
           // return (from a in re1.Split(re2.Replace(re3.Replace(re4.Replace(re5.Replace(str.ToLower(), "-"), ""), " "), "")) where a.Trim() != "" select a.Trim()).ToArray();

        }

        /// <summary>
        /// ³����� ������ ��� � �����.
        /// </summary>
        /// <param name="str">���� html-�������.</param>
        /// <param name="nameOfFile">����� �����</param>
        /// <returns></returns>
        public List<string> attributes(string str, string nameOfFile)
        {
            List<string> page = new List<string>();
            page.Add(str);
            String[] pageText = page.ToArray();

            List<string> infOfDocs = new List<string>();

            //adding name of File
            Regex name = new Regex("<meta property=\"og:title\" content=\"(.*)\">");
            Regex name2 = new Regex("<meta name=\"description\" content=\"(.*)\">"); // � 287 , � 490 
            Regex apostrophe = new Regex("&#39;");
            Regex quot = new Regex("&quot;"); 
            string res = "";
            foreach (var g in pageText)
            {
                if (name.IsMatch(g) && g.Contains("&lt;span class=") == false) 
                {
                    res += (name.Match(g).Groups[1].Value);
                }
                else if (name2.IsMatch(g))
                {
                    res += (name2.Match(g).Groups[1].Value);
                }

                if (res.Contains("&#39;"))
                {
                    res = apostrophe.Replace(res, "'");
                }
                if (res.Contains("&quot;"))
                {
                    res = quot.Replace(res, "\"");
                }

                infOfDocs.Add(res);
                

            }

            //adding �
            infOfDocs.Add(nameOfFile);

            //adding type of document
            Regex typeOfDoc = new Regex("<meta property=\"og:description\" content=\"(\\w+)(;)?(\\s(\\w+(;)?(,)?))+\\s(.{10})\\s�\\s(.+)\">");
            Regex typeOfDoc2 = new Regex("<span class=\"nh\">(\\w+)(\\s(\\w+))+</span>"); // � 287, � 490
            res = "";

            foreach (var g in pageText)
            {
                if (typeOfDoc.IsMatch(g) ||typeOfDoc2.IsMatch(g))
                {
                    string match1 = typeOfDoc.Match(g).Groups[1].Value;
                    string match2 = typeOfDoc2.Match(g).Groups[1].Value;
                    if(match2 != "")
                    {
                        infOfDocs.Add(match2);
                    }
                    else
                    {
                        infOfDocs.Add(match1);
                    }
                }
            }
        

            //adding publishing
           
            
            Regex publish1 = new Regex("<ul class=\"txttree sticky-top\">\\s+<li><a href=\"(.+)\"\\s+title=\"(.+)\"\\sclass=\"stru\"><span style=\"(.+)<b>(.+)\\s�������");
            Regex publish2 = new Regex("<ul class=\"txttree sticky-top\">\\s+<li><a href=\"(.+)\"\\s+title=\"(.+)\\s�������\"");
            Regex publish3 = new Regex("<a href=\"(.+)\"\\s+class=\"logo-text\">\\s+<div class=\"title\">\\s+(.+)\\s������"); // � 287, � 490
           // Regex president = new Regex("<div><a name=\"o\\w+\"\\sdata-tree=\"tb_1:nz_1\"><.a>\\s(.+)������"); // � 53_95, 300_96

            res = "";
            foreach (var g in pageText)
            {
                if (publish1.IsMatch(g)) 
                {

                    if (publish1.Match(g).Groups[4].Value == "�����")
                    {
                        res += "�������� ����";
                    }
                    else if (publish1.Match(g).Groups[4].Value == "���� ����������")
                    {
                        res += "���������";
                    }
                    else
                    {
                        res += publish1.Match(g).Groups[4].Value;
                        res = FirstUpper(res);
                    }
                     
                }
                else if (publish2.IsMatch(g))
                {
                   res += (publish2.Match(g).Groups[2].Value);
                   res = FirstUpper(res);
                }
                else if (publish3.IsMatch(g)) 
                {
                    res += (publish3.Match(g).Groups[2].Value);
                }
              
            }
            infOfDocs.Add(res + " ������");

            //adding date
            Regex dateOfAccept1 = new Regex("<title>.+[��|��] (\\d{2}.\\d{2}.\\d{4})");
            Regex dateOfAccept2 = new Regex("<div class=\"row\">\\s+<div class=\"col-md-4 col-sm-4\">\\s+((\\w+\\s+)+)����");
            res = "";
            foreach (var g in pageText)
            {
                if (dateOfAccept1.IsMatch(g)) 
                {
                    res += (dateOfAccept1.Match(g).Groups[1].Value);
                    infOfDocs.Add(res);
                }
                else if (dateOfAccept2.IsMatch(g))
                {
                    res += (dateOfAccept2.Match(g).Groups[1].Value);

                    string[] temp = res.Split(' ');

                    switch(temp[1].ToString().ToLower())
                    {
                        case "����":
                            infOfDocs.Add(NameMonthToNumber(res, ".01."));
                            break;
                        case "������":
                            infOfDocs.Add(NameMonthToNumber(res, ".02."));
                            break;
                        case "�������":
                            infOfDocs.Add(NameMonthToNumber(res, ".03."));
                            break;
                        case "�����":
                            infOfDocs.Add(NameMonthToNumber(res, ".04."));
                            break;
                        case "������":
                            infOfDocs.Add(NameMonthToNumber(res, ".05."));
                            break;
                        case "������":
                            infOfDocs.Add(NameMonthToNumber(res, ".06."));
                            break;
                        case "�����":
                            infOfDocs.Add(NameMonthToNumber(res, ".07."));
                            break;
                        case "������":
                            infOfDocs.Add(NameMonthToNumber(res, ".08."));
                            break;
                        case "�������":
                            infOfDocs.Add(NameMonthToNumber(res, ".09."));
                            break;
                        case "������":
                            infOfDocs.Add(NameMonthToNumber(res, ".10."));
                            break;
                        case "���������":
                            infOfDocs.Add(NameMonthToNumber(res, ".11."));
                            break;
                        case "������":
                            infOfDocs.Add(NameMonthToNumber(res, ".12."));
                            break;
                    }
                }
            }   

            return infOfDocs;
        }

        public string NameMonthToNumber(string res,string numberOfMonth)
        {
            string[] temp = res.Split(' ');

            res = res.Replace(temp[1], numberOfMonth);
            res = res.Replace(" ", "");
            return res;
        }

        public static string FirstUpper(string str)
        {
            string[] s = str.Split(' ');

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length > 1)
                    s[i] = s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                else s[i] = s[i].ToUpper();
            }
            return string.Join(" ", s);
        }
    }
    
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     