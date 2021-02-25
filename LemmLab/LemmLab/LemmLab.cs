using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     