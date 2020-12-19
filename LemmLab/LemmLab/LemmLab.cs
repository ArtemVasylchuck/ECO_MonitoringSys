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
        public string[] ToWordsHTML(string str)
        {
            //??????�������� ��������� 
            Regex re1 = new Regex("(\\s)|[.,:;\"()]");
            Regex re2 = new Regex("<(.|\\s|&quot)*?>");
            Regex re3 = new Regex("<style>(.|\\s)*?<\\/style>");
            //Regex re4 = new Regex(@"^\w$");
            return (from a in re1.Split(re2.Replace(re3.Replace(str.ToLower(), ""), "")) 
                    where a.Trim() != "" 
                    select a.Trim())
                    .ToArray();
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     