﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Data;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Text;
using HelpModule;

namespace Experts_Economist
{
    public partial class Dovidka : Form
    {

        private string tableName;
        private int id_of_exp = 0;
        private DBManager db = new DBManager();
        private List<RadioButton> radioButtons;
        private RadioButton oldValue = new RadioButton();
        private delegate void GDKAction();
        private delegate bool TextBoxErrorCondition(TextBox tb);
        GDKAction lambda;             //лямбда-функція, яка буде генеруватися для кожного RadioButton і виконується при натисканні кнопки "Виконати"

        //Названия колонок для различных таблиц.
        //Идёт в виде: первый массив названия, а второй - описание. Если описания нет ставится null и оно
        //дублирует название колонки.
        //Возможно вынести в отдельный файл или делать выборку с настройкой для каждой колонки псевдонима.
        //Например, SELECT code as Код ...
        private readonly Dictionary<string, KeyValuePair<string[], string[]>> tableColumnsNamesWithDescription =
            new Dictionary<string, KeyValuePair<string[], string[]>>
            {
                {
                    "gdk", new KeyValuePair<string[], string[]>
                    (
                    new string[] { "Код", "Максимально разове ГДК", "Середньодобове ГДК", "Клас небезпеки", "ОБРВ", "Середовище" },
                    new string[] { null, null, null, null, "(ОБРВ) Орієнтовно безпечний рівень впливу забруднючої середовище речовини", "Середовище, для якого визначені показники" }
                    )
                },
                {
                    "elements", new KeyValuePair<string[], string[]>
                    (
                    new string[] { "Код", "Назва", "Коротка назва", "Одиниця виміру", "rigid", "ЛОР", "Вуглець", "Формула", "УЧІР"},
                    new string[] { null, null, null, null, "Опис та назва відсутні. Необхідно додати опис та назву.", "Летючі органічні речовини", null, null, "Унікальний чисельний ідентифікатор речовин" }
                    )
                },
                {
                    "environment", new KeyValuePair<string[], string[]>
                    (
                    new string[] { "Идентифікатор", "Назва", "До якого середовища прикріплена", },
                    new string[] { null, null, "До якого середовища прикріплена (або є підсередовищем)" }
                    )
                },
                {
                    "tax_values", new KeyValuePair<string[], string[]>
                    (
                    new string[] { "Ідентифікатор елементу", "Податок", "Середовище", },
                    new string[] { null, null, null }
                    )
                },
            };


        ~Dovidka()
        {
            db.Disconnect();
            db = null;
        }

        public Dovidka(string tableName, int id_of_exp)
        {
            InitializeComponent();
            this.tableName = tableName;
            this.id_of_exp = id_of_exp;
            if (id_of_exp == 0)
            {
                doneBtn.Visible = true;
                workMode.Visible = true;
            }
            else
            {
                doneBtn.Visible = false;
                workMode.Visible = false;
            }
        }

        //____Спеціальні функції_____

        //Створення спеціального Label з автоматичним розміром.
        private Label createAutoSizedLabel(string txt)
        {
            Label label = new Label
            {
                Text = txt,
                AutoSize = true
            };
            return label;
        }

        //Створення спеціального поля з шириною 114 та вказаним ім'ям.
        private TextBox createTextBox(string txt)
        {
            TextBox text = new TextBox
            {
                Name = txt,
                Width = 114
            };
            return text;
        }

        //Завантаження даних з SQL-таблиці experts.elements у таблицю gdkDataView.
        private void loadDataToTable()
        {
            MySqlConnection connection = new MySqlConnection(db.connectionString);

            try
            {
                connection.Open();

                string query = string.Empty;
                var lowerTableName = tableName.ToLower();

                if (lowerTableName == "gdk")
                {
                    query = "SELECT gdk.code, gdk.mpc_m_ot, gdk.mpc_avrg_d, gdk.danger_class, gdk.tsel, environment.name FROM " +
                            tableName +
                            " LEFT JOIN environment ON environment.id = gdk.environment";
                }
                else if (lowerTableName == "environment")
                {
                    query = "SELECT env1.id, env1.name, env2.name " +
                            "FROM environment as env1 " +
                            "LEFT JOIN environment as env2 ON env2.Id = env1.AttachEnv ORDER BY env1.id";
                }
                else
                {
                    query = $"select * from {tableName}";
                }

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, connection);
                DataSet DS = new DataSet();
                mySqlDataAdapter.Fill(DS);
                gdkDataGrid.DataSource = DS.Tables[0];

                if (tableColumnsNamesWithDescription.ContainsKey(lowerTableName))
                {
                    var columnsNames = tableColumnsNamesWithDescription[lowerTableName];
                    var length = columnsNames.Key.Length > gdkDataGrid.Columns.Count ? gdkDataGrid.Columns.Count : columnsNames.Key.Length;

                    for (int i = 0; i < columnsNames.Key.Length; i++)
                    {
                        var column = gdkDataGrid.Columns[i];

                        column.HeaderText = columnsNames.Key[i];
                        column.HeaderCell.ToolTipText = columnsNames.Value[i] ?? column.HeaderText;
                    }
                }


                //close connection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        //функція для завантаження даних з бд у комбобокс
        public static void populateComboBox(ref ComboBox combo, string connectionString, string table, string field, string condition = null)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlDataAdapter mySqlDataAdapter;

            try
            {
                connection.Open();
                if (condition == null)
                    mySqlDataAdapter = new MySqlDataAdapter($"select {field} from {table}", connection);
                else
                    mySqlDataAdapter = new MySqlDataAdapter($"select {field} from {table} where {condition}", connection);
                DataSet DS = new DataSet();
                mySqlDataAdapter.Fill(DS);
                combo.DataSource = DS.Tables[0];
                combo.DisplayMember = field;

                //close connection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        //функції перевірки на помилки
        private bool checkTextBoxesForAnErrorWhere(IEnumerable<TextBox> textBoxes, TextBoxErrorCondition doesHaveAnError)
        {
            return textBoxes.Any(textBox => doesHaveAnError(textBox));
        }
        private string formAnErrorStringWhere(IEnumerable<TextBox> textBoxes, TextBoxErrorCondition doesHaveAnErrorIn)
        {

            string errorStr = String.Empty;
            var errorInTextBoxes = textBoxes.Where(textBox => doesHaveAnErrorIn(textBox));
            foreach (var textBox in errorInTextBoxes)
            {
                errorStr += $"{textBox.Name}, ";
            }
            return Regex.Replace(errorStr, @", $", "");
        }
        private bool checkTextBoxesForAnError()
        {
            //створюємо умову перевірки полів вводу
            TextBoxErrorCondition textBoxesIsEmpty = textBox => textBox.Text.Trim() == "";
            TextBoxErrorCondition textBoxesIsNotInt = textBox => !double.TryParse(textBox.Text.Trim(), out double result);

            //отримуємо всі поля вводу
            var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>();

            //змінна, яка призначеня для зберігання числових полів вводу
            IEnumerable<TextBox> numericTextBoxes = Enumerable.Empty<TextBox>();

            //змінна для виводу помилок
            string errorStr = String.Empty;

            //знаходимо всі поля, які повинні містити числові значення
            if (tableName == "elements")
                numericTextBoxes = textBoxes.Where(textBox => { return textBox.Name == "code"; });
            else if (tableName == "gdk")
                numericTextBoxes = textBoxes;
            else if (tableName == "emissions")
                numericTextBoxes = textBoxes.Where(textBox => { return textBox.Name != "device" || textBox.Name != "day1" || textBox.Name != "day2"; });
            else if (tableName == "tax_values")
                numericTextBoxes = textBoxes;

            //якщо числове поле містить символ "," при введені чисел з плаваючою точнкою то замінити на "."
            numericTextBoxes.ToList().ForEach((textBox) => textBox.Text = textBox.Text.Trim().Replace('.', ','));

            if (checkTextBoxesForAnErrorWhere(textBoxes, textBoxesIsEmpty))                    //Якщо хоча б один TextBox пустий, то формуємо строку з іменем пустих полів 
            {
                errorStr = $"Декілька або одне поле має невірний формат введених даних\nПоля: {formAnErrorStringWhere(textBoxes, textBoxesIsEmpty)}";
            }
            else if (checkTextBoxesForAnErrorWhere(numericTextBoxes, textBoxesIsNotInt))        //Якщо хоча б в один TextBox введено не числове значення, то формуємо строку з іменами полів де є помилки
            {
                errorStr = $"Декілька або одне поле має невірний формат введених даних\nПоля: {formAnErrorStringWhere(numericTextBoxes, textBoxesIsNotInt)}";
            }

            //повертаємо значення false, що свідчить про відсутність помилок у полях вводу
            else { numericTextBoxes.ToList().ForEach((textBox) => textBox.Text = textBox.Text.Trim().Replace(',', '.')); return false; }

            //виводимо помилку, якщо одна з функцій перевірки 
            MessageBox.Show(errorStr, "УВАГА!");
            return true;
        }

        //Функція для відстежування натискань на RadioButtons на панелі "Режим роботи".
        //В залежності від обраного RadioButton додаються спеціальний набір компонетів
        //для взаємодії з таблицейю.
        private void handleRadioButtonChange(object sender, EventArgs e)
        {
            //конвертуємо в радіобатн
            RadioButton selected = (RadioButton)sender;

            //змінна для зберігання компонентів (TextBox, CheckBox, RadioButton)
            List<Control> controls = new List<Control>();
            lambda = () => { return; };

            if (oldValue.Equals(selected)) return;    //при зміні комбобоксу код може виконатися 2 рази (так як дія зміни виконується 2 рази "стареОбране->нове обране" замість потрібного "нове обране")
            else if (oldValue == searchRadio) loadDataToTable();       //кнопка пошуку своїми діями змінює таблицю і тому є необхідність перезаповнити її 

            //очищення робочої області(панель над таблицею) перед формуванням нових компонентів
            flowLayoutPanel1.Controls.Clear();

            //обираємо режим роботи
            switch (selected.Name)
            {
                case "searchRadio":
                    {
                        doneBtn.Text = "Виконати";

                        controls = new List<Control>                                               //формуємо набір компонентів
                            {
                                createAutoSizedLabel("Код"), new TextBox(){Name="code"},
                                createAutoSizedLabel("Назва"), new TextBox(){Name="name"},
                                createAutoSizedLabel("Коротка назва"), new TextBox(){Name="short"},
                                createAutoSizedLabel("Одиниця виміру"), new TextBox(){Name="measure"},
                                createAutoSizedLabel("Формула"), new TextBox(){Name="formual"},
                                createAutoSizedLabel("УЧІР"), new TextBox(){Name="cas"}, createAutoSizedLabel(""),
                                new CheckBox(){Name="rigid", Text="rigid",AutoSize = true},
                                new CheckBox(){Name="voc", Text="ЛОР",AutoSize = true},
                                new CheckBox(){Name="hydro",Text="Вуглець", AutoSize = true},
                            };

                        lambda = () =>                  //створюємо подію для цього режиму роботи
                        {
                            if (gdkDataGrid.DataSource == null && !(gdkDataGrid.DataSource is DataTable))
                            {
                                MessageBox.Show($"Неможливо виконати пошук.\nТаблиця порожня", "Помилка!",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            //отримуємо всі поля вводу
                            var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();

                            //отримуємо всі чекбокси вводу
                            var checkBoxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToArray();

                            //вказуємо поля, які потрібно доти
                            string[] fieldNames = { "code", "name", "short_name", "measure", "rigid", "voc", "hydrocarbon", "formula", "cas" };

                            //вказуємо відповідне значення цих полів
                            string[] fieldValues = {$"{textBoxes[0].Text}",$"'{textBoxes[1].Text}'",$"'{textBoxes[2].Text}'",
                                $"'{textBoxes[3].Text}'",$"{checkBoxes[0].Checked}",$"{checkBoxes[1].Checked}",
                                $"{checkBoxes[2].Checked}",$"'{textBoxes[4].Text}'",$"'{textBoxes[5].Text}'" };
                            try
                            {
                                var result = db.GetRows("elements", "*",
                                                        $"({string.IsNullOrEmpty(textBoxes[0].Text)} OR code = '{textBoxes[0].Text}' OR code LIKE '{textBoxes[0].Text}') AND " +
                                                        $"({string.IsNullOrEmpty(textBoxes[1].Text)} OR name = '{textBoxes[1].Text}' OR name LIKE '{textBoxes[1].Text}') AND " +
                                                        $"({string.IsNullOrEmpty(textBoxes[2].Text)} OR short_name = '{textBoxes[2].Text}' OR short_name LIKE '{textBoxes[2].Text}') AND " +
                                                        $"({string.IsNullOrEmpty(textBoxes[3].Text)} OR measure = '{textBoxes[3].Text}' OR measure LIKE '{textBoxes[3].Text}') AND " +
                                                        $"({checkBoxes[0].Checked} OR true) AND " +
                                                        $"({checkBoxes[1].Checked} OR true) AND " +
                                                        $"({checkBoxes[2].Checked} OR true) AND " +
                                                        $"({string.IsNullOrEmpty(textBoxes[4].Text)} OR formula = '{textBoxes[4].Text}' OR formula LIKE '{textBoxes[4].Text}') AND " +
                                                        $"({string.IsNullOrEmpty(textBoxes[5].Text)} OR cas = '{textBoxes[5].Text}') OR cas LIKE '{textBoxes[5].Text}'");

                                if (result.Count == 0)
                                {
                                    MessageBox.Show("Дані відсутні. Введіть більш корректні дані.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                DataTable data = (DataTable)gdkDataGrid.DataSource;
                                data.Rows.Clear();
                                object[] tempArr = new object[result[0].Count];

                                for (int i = 0; i < result.Count; i++)
                                {
                                    for (int j = 0; j < result[i].Count; j++)
                                    {
                                        tempArr[j] = result[i][j];
                                    }

                                    data.Rows.Add(tempArr);
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show($"Неможливо виконати пошук\n{exception.Message}", "Помилка!");
                            }
                        };

                        break;
                    }
                case "addRadio":
                    {
                        doneBtn.Text = "Додати";

                        if (tableName == "elements")
                        {
                            controls = new List<Control>                                               //формуємо набір компонентів
                            {
                                createAutoSizedLabel("Код"), new TextBox(){Name="code"},
                                createAutoSizedLabel("Назва"), new TextBox(){Name="name"},
                                createAutoSizedLabel("Коротка назва"), new TextBox(){Name="short"},
                                createAutoSizedLabel("Одиниця виміру"), new TextBox(){Name="measure"},
                                createAutoSizedLabel("Формула"), new TextBox(){Name="formual"},
                                createAutoSizedLabel("УЧІР"), new TextBox(){Name="cas"}, createAutoSizedLabel(""),
                                new CheckBox(){Name="rigid", Text="rigid",AutoSize = true},
                                new CheckBox(){Name="voc", Text="ЛОР",AutoSize = true},
                                new CheckBox(){Name="hydro",Text="Вуглець", AutoSize = true},
                            };

                            lambda = () =>                  //створюємо подію для цього режиму роботи
                            {
                                //отримуємо всі поля вводу
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();

                                //отримуємо всі чекбокси вводу
                                var checkBoxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToArray();

                                //вказуємо поля, які потрібно доти
                                string[] fieldNames = { "code", "name", "short_name", "measure", "rigid", "voc", "hydrocarbon", "formula", "cas" };

                                //вказуємо відповідне значення цих полів
                                string[] fieldValues = {$"{textBoxes[0].Text}",$"'{textBoxes[1].Text}'",$"'{textBoxes[2].Text}'",
                                $"'{textBoxes[3].Text}'",$"{checkBoxes[0].Checked}",$"{checkBoxes[1].Checked}",
                                $"{checkBoxes[2].Checked}",$"'{textBoxes[4].Text}'",$"'{textBoxes[5].Text}'" };
                                try
                                {
                                    //додаємо значення до бд
                                    db.InsertToBDWithoutId("elements", fieldNames, fieldValues);
                                }
                                catch (Exception exception)
                                {
                                    MessageBox.Show($"Неможливо створити запис\n{exception.Message}", "Помилка!");
                                }
                            };
                        }
                        else if (tableName == "gdk")
                        {
                            ComboBox code_combo = new ComboBox() { Name = "code" };
                            populateComboBox(ref code_combo, db.connectionString, "elements", "code");
                            ComboBox environment_combo = new ComboBox() { Name = "environment" };
                            populateComboBox(ref environment_combo, db.connectionString, "environment", "name");
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Код"), code_combo,
                                createAutoSizedLabel("Максимально разове ГДК"), new TextBox(){ Name = "mpc_m_ot"},
                                createAutoSizedLabel("Середньодобове ГДК"), new TextBox(){ Name = "mpc_avrg_d"},
                                createAutoSizedLabel("Клас небезпеки"), new TextBox(){ Name="danger"},
                                createAutoSizedLabel("ОБРВ"), new TextBox(){ Name="tsel"},
                                createAutoSizedLabel("Середовище"), environment_combo,
                            };

                            lambda = () =>
                            {
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                var comboBoxes = flowLayoutPanel1.Controls.OfType<ComboBox>().ToArray();
                                var env_ID = db.GetValue("environment", "id", $"name = '{comboBoxes[1].Text}'");        //отримуємо ідентифікатор "середовища" так як таблиця очікує числов значення цього поля
                                try
                                {
                                    db.InsertToBD("gdk", $"{comboBoxes[0].Text}, {textBoxes[0].Text}, {textBoxes[1].Text}, {textBoxes[2].Text}, {textBoxes[3].Text}, {env_ID}");
                                }
                                catch (Exception exception)
                                {
                                    MessageBox.Show($"Неможливо створити запис\n{exception.Message}", "Помилка!");
                                }
                            };
                        }
                        else if (tableName == "environment")
                        {
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("ID"), new TextBox(){Name="id"},
                                createAutoSizedLabel("Назва"), new TextBox(){Name="name"}
                            };

                            lambda = () =>
                            {
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                string[] fieldNames = { "id", "name" };
                                string[] fieldValues = { $"{textBoxes[0].Text}", $"'{textBoxes[1].Text}'" };

                                try
                                {
                                    db.InsertToBD("environment", fieldNames, fieldValues);
                                }
                                catch (Exception exception)
                                {
                                    MessageBox.Show($"Неможливо створити запис\n{exception.Message}", "Помилка!");
                                }
                            };
                        }
                        else if (tableName == "tax_values")
                        {
                            ComboBox code_combo = new ComboBox() { Name = "id_of_element" };
                            populateComboBox(ref code_combo, db.connectionString, "elements", "name");
                            ComboBox env_combo = new ComboBox() { Name = "environment" };
                            populateComboBox(ref env_combo, db.connectionString, "environment", "name");
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Елемент"), code_combo,
                                createAutoSizedLabel("Податкова вартість"), new TextBox(){Name="tax value"},
                                createAutoSizedLabel("Середовище"), env_combo
                            };
                            lambda = () =>
                            {
                                var comboBoxes = flowLayoutPanel1.Controls.OfType<ComboBox>().ToArray();
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                var elem_ID = db.GetValue("elements", "code", $"name = '{comboBoxes[0].Text}'");
                                var env_ID = db.GetValue("environment", "id", $"name = '{comboBoxes[1].Text}'");        //отримуємо ідентифікатор "середовища" так як таблиця очікує числов значення цього поля

                                try
                                {
                                    db.InsertToBD("tax_values", $"{elem_ID}, {textBoxes[0].Text}, {env_ID}");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Неможливо створити запис\n{ex.Message}", "Помилка!");
                                }
                            };
                        }
                        break;
                    }
                case "editRadio":
                    {
                        doneBtn.Text = "Редагувати";

                        if (tableName == "elements")
                        {
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Код"), new TextBox(){Name="code"},
                                createAutoSizedLabel("Назва"), new TextBox(){Name="name"},
                                createAutoSizedLabel("Коротка назва"), new TextBox(){Name="short"},
                                createAutoSizedLabel("Одиниця виміру"), new TextBox(){Name="measure"},
                                createAutoSizedLabel("Формула"), new TextBox(){Name="formual"},
                                createAutoSizedLabel("УЧІР"), new TextBox(){Name="cas"}, createAutoSizedLabel(""),
                                new CheckBox(){Name="rigid", Text="rigid",AutoSize = true},
                                new CheckBox(){Name="voc", Text="ЛОР",AutoSize = true},
                                new CheckBox(){Name="hydro",Text="Вуглець", AutoSize = true},
                            };

                            lambda = () =>
                            {
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                var checkBoxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToArray();
                                try
                                {
                                    int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    var record_ID = gdkDataGrid.Rows[rowIndex].Cells[0].Value.ToString();

                                    //MessageBox.Show(record_ID.ToString());
                                    string[] colNames = { "code", "name", "short_name", "measure", "rigid", "voc", "hydrocarbon", "formula", "cas" };
                                    string[] colValues = {$"{textBoxes[0].Text}",$"'{textBoxes[1].Text}'",$"'{textBoxes[2].Text}'",
                                    $"'{textBoxes[3].Text}'",$"{checkBoxes[0].Checked}",$"{checkBoxes[1].Checked}",
                                    $"{checkBoxes[2].Checked}",$"'{textBoxes[4].Text}'",$"'{textBoxes[5].Text}'" };
                                    //string[] colValues = { record_ID.ToString(), $"'{nameTB.Text.Trim()}'", $"'{categoryCB.Text.Trim()}'", GDK_TB.Text.Trim(), $"'{measureTB.Text.Trim() }'", minTB.Text.Trim(), maxTB.Text.Trim() };
                                    db.UpdateRecord("elements", colNames, colValues);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Помилка оновлення даних!\n{ex.Message}", "Помилка");
                                }
                            };
                        }
                        else if (tableName == "gdk")
                        {
                            ComboBox code_combo = new ComboBox() { Name = "code" };
                            populateComboBox(ref code_combo, db.connectionString, "elements", "code");
                            ComboBox environment_combo = new ComboBox() { Name = "environment" };
                            populateComboBox(ref environment_combo, db.connectionString, "environment", "name");
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Код"), code_combo,
                                createAutoSizedLabel("Максимально разове ГДК"), new TextBox(){ Name = "mpc_m_ot"},
                                createAutoSizedLabel("Середньодобове ГДК"), new TextBox(){ Name = "mpc_avrg_d"},
                                createAutoSizedLabel("Клас небезпеки"), new TextBox(){ Name="danger"},
                                createAutoSizedLabel("ОБРВ"), new TextBox(){ Name="tsel"},
                            };

                            lambda = () =>
                            {
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                var comboBoxes = flowLayoutPanel1.Controls.OfType<ComboBox>().ToArray();

                                textBoxes.ToList().ForEach(textBox => textBox.Text = textBox.Text.Replace(',', '.'));

                                try
                                {
                                    int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    var record_ID = gdkDataGrid.Rows[rowIndex].Cells[0].Value.ToString();

                                    var environment_ID = db.GetValue("environment", "id", $"name = '{comboBoxes[1].Text}'");

                                    string[] colNames = { "code", "mpc_m_ot", "mpc_avrg_d", "danger_class", "tsel", "environment" };
                                    string[] colValues = { $"{comboBoxes[0].Text}", $"{textBoxes[0].Text}", $"{textBoxes[1].Text}", $"{textBoxes[2].Text}", $"{textBoxes[3].Text}", $"{environment_ID}" };

                                    db.UpdateRecord("gdk", colNames, colValues);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Помилка оновлення даних!\n{ex.Message}", "Помилка");
                                }
                            };
                        }
                        else if (tableName == "environment")
                        {
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("ID"), new TextBox(){Name="id"},
                                createAutoSizedLabel("Назва"), new TextBox(){Name="name"}
                            };

                            lambda = () =>
                            {
                                try
                                {
                                    int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    var record_ID = db.GetValue("environment", "id", $"name = '{gdkDataGrid.Rows[rowIndex].Cells[1].Value.ToString()}'");
                                    var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();

                                    string[] colNames = { "id", "name" };
                                    string[] colValues = { $"{textBoxes[0].Text}", $"'{textBoxes[1].Text}'" };

                                    db.UpdateRecord("environment", colNames, colValues);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Помилка оновлення даних!\n{ex.Message}", "Помилка");
                                }
                            };
                        }
                        else if (tableName == "tax_values")
                        {
                            ComboBox code_combo = new ComboBox() { Name = "id_of_element" };
                            populateComboBox(ref code_combo, db.connectionString, "elements", "name");
                            ComboBox env_combo = new ComboBox() { Name = "environment" };
                            populateComboBox(ref env_combo, db.connectionString, "environment", "name");
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Елемент"), code_combo,
                                createAutoSizedLabel("Податкова вартість"), new TextBox(){Name="tax"},
                                createAutoSizedLabel("Середовище"), env_combo
                            };
                            lambda = () =>
                            {
                                try
                                {
                                    var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                    var comboBoxes = flowLayoutPanel1.Controls.OfType<ComboBox>().ToArray();

                                    textBoxes.ToList().ForEach(textBox => textBox.Text = textBox.Text.Replace(',', '.'));

                                    string[] colNames = { "id_of_element", "tax", "environment" };
                                    var tmp = db.GetValue("elements", "code", $"name = '{comboBoxes[0].Text}'");
                                    var env_ID = db.GetValue("environment", "id", $"name = '{comboBoxes[1].Text}'");
                                    string[] colValues = { $"{tmp}", $"{textBoxes[0].Text}", $"{env_ID}" };

                                    db.UpdateRecord("tax_values", colNames, colValues);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Неможливо створити запис\n{ex.Message}", "Помилка!");
                                }
                            };
                        }
                        break;
                    }
                case "deleteRadio":
                    {
                        doneBtn.Text = "Видалити";

                        if (tableName == "elements")
                        {
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Код"), new TextBox(){Name="code"}
                                //createAutoSizedLabel("Name"), new TextBox(){Name="name"},
                                //createAutoSizedLabel("Short"), new TextBox(){Name="short"},
                                //createAutoSizedLabel("Measure"), new TextBox(){Name="measure"},
                                //createAutoSizedLabel("Formula"), new TextBox(){Name="formual"},
                                //createAutoSizedLabel("CAS"), new TextBox(){Name="cas"}, createAutoSizedLabel(""),
                                //new CheckBox(){Name="rigid", Text="rigid",AutoSize = true},
                                //new CheckBox(){Name="voc", Text="voc",AutoSize = true},
                                //new CheckBox(){Name="hydro",Text="hydro", AutoSize = true},
                            };

                            lambda = () =>
                            {
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                var checkBoxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToArray();
                                try
                                {
                                    int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    var record_ID = gdkDataGrid.Rows[rowIndex].Cells[0].Value.ToString();

                                    //MessageBox.Show(record_ID.ToString());

                                    //string[] colValues = { record_ID.ToString(), $"'{nameTB.Text.Trim()}'", $"'{categoryCB.Text.Trim()}'", GDK_TB.Text.Trim(), $"'{measureTB.Text.Trim() }'", minTB.Text.Trim(), maxTB.Text.Trim() };
                                    db.DeleteFromDB("elements", "code", record_ID.ToString());

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Помилка оновлення даних!\n{ex.Message}", "Помилка");
                                }
                            };
                        }
                        else if (tableName == "gdk")
                        {
                            ComboBox code_combo = new ComboBox();
                            populateComboBox(ref code_combo, db.connectionString, "elements", "code");
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Code"), code_combo,
                            };

                            lambda = () =>
                            {
                                var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();
                                try
                                {
                                    int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    var record_ID = gdkDataGrid.Rows[rowIndex].Cells[0].Value.ToString();

                                    db.DeleteFromDB("gdk", "code", record_ID.ToString());

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Помилка оновлення даних!\n{ex.Message}", "Помилка");
                                }
                            };
                        }
                        else if (tableName == "environment")
                        {
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("ID"), new TextBox(){Name="id"},
                                createAutoSizedLabel("Назва"), new TextBox(){Name="name"}
                            };

                            lambda = () =>
                            {
                                try
                                {
                                    int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    var record_ID = db.GetValue("environment", "id", $"name = '{gdkDataGrid.Rows[rowIndex].Cells[1].Value.ToString()}'");
                                    var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();

                                    string[] colNames = { "id", "name" };
                                    string[] colValues = { $"{textBoxes[0].Text}", $"'{textBoxes[1].Text}'" };

                                    db.DeleteFromDB("environment", "id", record_ID.ToString());
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Помилка оновлення даних!\n{ex.Message}", "Помилка");
                                }
                            };
                        }
                        else if (tableName == "tax_values")
                        {
                            ComboBox code_combo = new ComboBox();
                            populateComboBox(ref code_combo, db.connectionString, "tax_values", "id_of_element");
                            controls = new List<Control>
                            {
                                createAutoSizedLabel("Ідентифікатор елементу"), code_combo
                            };
                            lambda = () =>
                            {

                                try
                                {
                                    //int rowIndex = gdkDataGrid.CurrentCell.RowIndex;
                                    //var record_ID = gdkDataGrid.Rows[rowIndex].Cells[0].Value.ToString();
                                    var cBoxes = flowLayoutPanel1.Controls.OfType<ComboBox>().ToArray();
                                    var record_ID = cBoxes[0].Text;

                                    db.DeleteFromDB("tax_values", "id_of_element", record_ID.ToString());
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Неможливо створити запис\n{ex.Message}", "Помилка!");
                                }
                            };
                        }
                        break;
                    }
                default:
                    break;
            }

            SetEventHandlersForControls(ref controls);
            //після ініціалізації всіх компонетів додаємо їх на робочу панель
            flowLayoutPanel1.Controls.AddRange(controls.ToArray());

            //позбавлення від тимчасових компонентів так як їх вже було додано до робочої панелі
            controls = null;
            oldValue = selected;
        }

        //_____Сгенеровані функції______

        private void DovidkaGDK_Load(object sender, EventArgs e)
        {
            loadDataToTable();
            radioButtons = new List<RadioButton> { searchRadio, addRadio, editRadio, deleteRadio };
        }


        //функція для виконання дії обраного режиму роботи
        private void doneBtn_Click(object sender, EventArgs e)
        {
            //якщо ініціалізовано подію
            if (lambda != null)
            {
                //якщо не обрано "пошук" та "видалення" то перевірити поля вводу на помилки
                if (!searchRadio.Checked && !deleteRadio.Checked)
                {
                    //якщо є помилки, то вийти з функції
                    if (checkTextBoxesForAnError())
                        return;
                }
                //якщо обрано "Пошук", то не оновлювати DataGridView так як саме в ній відображаються результати пошуку
                else if (searchRadio.Checked)
                {
                    lambda();
                    return;
                }

                //виконання події з оновленням таблиці
                lambda();
                loadDataToTable();
            }
            else
                MessageBox.Show("Оберіть режим роботи!", "Увага!");
        }

        //подія, яка виникає при натисканні правою кнопкою миші на таблицю
        private void gdkDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //якщо режим роботи "редагування" або "видалення"
            if (editRadio.Checked || deleteRadio.Checked)
            {
                try
                {
                    //отримуємо номер обраного рядка
                    int index = gdkDataGrid.CurrentCell.RowIndex;
                    var cells = gdkDataGrid.Rows[index].Cells;                                  //отримуємо масив комірок рядка
                    var textBoxes = flowLayoutPanel1.Controls.OfType<TextBox>().ToArray();      //отримуємо всі поля для вводу значень
                    var textCells = cells.OfType<DataGridViewTextBoxCell>().ToArray();          //отримуємо всі текстові поля таблиці
                    var comboBoxes = flowLayoutPanel1.Controls.OfType<ComboBox>().ToArray();

                    if (tableName == "gdk")
                    {
                        comboBoxes[0].Text = textCells[0].Value.ToString();

                        //заповнюємо все поля для вводу значеннями з таблиці
                        for (int i = 1; i <= textBoxes.Length; i++)
                        {
                            textBoxes[i - 1].Text = textCells[i].Value.ToString();
                        }

                        var checkBoxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToArray();    //отримуємо всі поля для вводу типу ChechBox
                        var checkCells = cells.OfType<DataGridViewCheckBoxCell>().ToArray();

                        //заповнюємо всі поля вводу значеннями з таблиці
                        for (int i = 0; i < checkBoxes.Length; i++)
                        {
                            checkBoxes[i].Checked = (bool)checkCells[i].Value;
                        }
                    }
                    else if (tableName == "tax_values")
                    {
                        comboBoxes[0].Text = db.GetValue("elements", "name", $"code = '{textCells[0].Value.ToString()}'").ToString();
                        textBoxes[0].Text = textCells[1].Value.ToString();
                        comboBoxes[1].Text = db.GetValue("environment", "name", $"id = '{textCells[2].Value.ToString()}'").ToString();
                    }
                    else
                    {
                        //заповнюємо все поля для вводу значеннями з таблиці
                        for (int i = 0; i < textBoxes.Length; i++)
                        {
                            textBoxes[i].Text = textCells[i].Value.ToString();
                        }

                        var checkBoxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToArray();    //отримуємо всі поля для вводу типу ChechBox
                        var checkCells = cells.OfType<DataGridViewCheckBoxCell>().ToArray();

                        //заповнюємо всі поля вводу значеннями з таблиці
                        for (int i = 0; i < checkBoxes.Length; i++)
                        {
                            checkBoxes[i].Checked = (bool)checkCells[i].Value;
                        }
                    }

                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show($"Не вдалося завантажити дані!\n{ex.Message} ", "Увага!");
                }
            }
        }

        private void gdkDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gdkDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.Value = e.Value is DBNull ? "-" : e.Value;
        }

        private void startTutorial_Click(object sender, EventArgs e)
        {
            var frm = new HelpToolTipForm(delegate
            {
                if (id_of_exp != 0)
                    return;
                if (!searchRadio.Checked && !addRadio.Checked && !editRadio.Checked && !deleteRadio.Checked)
                    StartTutorialProcess();
                else
                    StartTutorialRd();
            }, delegate
            {
                string page = "";
                if (this.tableName == "elements")
                {
                    page = "p13.html";
                }
                else if (this.tableName == "gdk")
                {
                    page = "p14.html";
                }
                else if (this.tableName == "environment")
                {
                    page = "p15.html";
                }
                else if (this.tableName == "emissions")
                {
                    page = "p16.html";
                }
                else if (this.tableName == "tax_values")
                {
                    page = "p17.html";
                }
                else
                {
                    return;
                }

                Help.ShowHelp(this, Config.PathToHelp, HelpNavigator.Topic, page);
            });
            frm.ShowDialog();

        }

        private void StartTutorialProcess()
        {
            new InteractiveToolTipCreator().CreateTips(new List<InteractiveToolTipModel>
            {
                new InteractiveToolTipModel
                {
                    Control = workMode,
                    Text = "Необхідно обрати режим роботи",
                    IsNotFinal = true,
                    AfterHandler = AfterWorkModeChecked
                }
            });
        }

        private void AfterWorkModeChecked()
        {
            if (!searchRadio.Checked && !addRadio.Checked && !editRadio.Checked && !deleteRadio.Checked)
            {
                searchRadio.CheckedChanged += WorkModeCheckedHandler;
                addRadio.CheckedChanged += WorkModeCheckedHandler;
                editRadio.CheckedChanged += WorkModeCheckedHandler;
                deleteRadio.CheckedChanged += WorkModeCheckedHandler;
            }
            else
                StartTutorialRd();
        }

        private void StartTutorialRd()
        {
            if (searchRadio.Checked)
            {
                tutorialSearch();
            }
            else if (addRadio.Checked)
            {
                tutorialAdd();
            }
            else if (editRadio.Checked)
            {
                tutorialEdit();
            }
            else if (deleteRadio.Checked)
            {
                tutorialDelete();
            }
        }

        private void WorkModeCheckedHandler(object sender, EventArgs e)
        {
            searchRadio.CheckedChanged -= WorkModeCheckedHandler;
            addRadio.CheckedChanged -= WorkModeCheckedHandler;
            editRadio.CheckedChanged -= WorkModeCheckedHandler;
            deleteRadio.CheckedChanged -= WorkModeCheckedHandler;
            var rd = (RadioButton)sender;
            if (rd == searchRadio)
            {
                tutorialSearch();
            }
            else if (rd == addRadio)
            {
                tutorialAdd();
            }
            else if (rd == editRadio)
            {
                tutorialEdit();
            }
            else if (rd == deleteRadio)
            {
                tutorialDelete();
            }
        }

        private void tutorialSearch()
        {
            new InteractiveToolTipCreator().CreateTips(new List<InteractiveToolTipModel>
            {
                new InteractiveToolTipModel
                {
                    Control = flowLayoutPanel1,
                    Text = "Для пошуку необхідно заповнити дані"
                },
                new InteractiveToolTipModel
                {
                    Control = doneBtn,
                    Text = "Щоб виконати пошук натисніть на кнопку \"Виконати\""
                }
            });
        }

        private void tutorialAdd()
        {
            new InteractiveToolTipCreator().CreateTips(new List<InteractiveToolTipModel>
            {
                new InteractiveToolTipModel
                {
                    Control = flowLayoutPanel1,
                    Text = "Для того щоб додати новий запис необхідно заповнити дані"
                },
                new InteractiveToolTipModel
                {
                    Control = doneBtn,
                    Text = "Натисніть на кнопку \"Додати\" щоб зберегти дані"
                }
            });
        }

        private void tutorialEdit()
        {
            new InteractiveToolTipCreator().CreateTips(new List<InteractiveToolTipModel>
            {
                new InteractiveToolTipModel
                {
                    Control = gdkDataGrid,
                    Text = "Для того щоб відредагувати запис необхідного обрати його з таблиці"
                },
                new InteractiveToolTipModel
                {
                    Control = flowLayoutPanel1,
                    Text = "Необхідно внести зміни в поля"
                },
                new InteractiveToolTipModel
                {
                    Control = doneBtn,
                    Text = "Щоб зберегти зміни натисніть на кнопку \"Редагувати\""
                }
            });
        }

        private void tutorialDelete()
        {
            new InteractiveToolTipCreator().CreateTips(new List<InteractiveToolTipModel>
            {
                new InteractiveToolTipModel
                {
                    Control = gdkDataGrid,
                    Text = "Для того щоб видалити запис, оберіть його з таблиці"
                },
                new InteractiveToolTipModel
                {
                    Control = doneBtn,
                    Text = "Щоб виконати видалення натисніть на кнопку \"Видалити\""
                }
            });
        }

        private void SetEventHandlersForControls(ref List<Control> controls)
        {
            foreach (Control control in controls)
            {
                if (control is TextBox tb)
                {
                    if (tb.Name == "code")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Порядковий номер {0} у таблиці (цілі числа)", tb);
                        };
                    }
                    else if (tb.Name == "short")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Коротка назва {0} (обмеження до 50 символів)", tb);
                        };
                    }
                    else if (tb.Name == "formual")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Формула {0} (обмеження до 100 символів)", tb);
                        };
                    }
                    else if (tb.Name == "name")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Повна назва {0} (обмеження до 100 символів)", tb);
                        };
                    }
                    else if (tb.Name == "measure")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Одиниця вимірювання {0} (обмеження до 20 символів)", tb);
                        };
                    }
                    else if (tb.Name == "cas")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Унікальний числовий ідентифікатор речовини (обмеження до 45 символів)", tb);
                        };
                    }
                    else if (tb.Name == "id")
                    {
                        tb.Click += delegate
                        {
                            CreateToolTip("Порядковий номер середовища в таблиці", tb);
                        };
                    }
                }
                else if (control is CheckBox checkBox)
                {
                    if (checkBox.Name == "rigid")
                    {
                        checkBox.MouseHover += delegate
                        {
                            CreateToolTip("Розчинність (поставити відмітку, якщо речовина розчинна)", checkBox);
                        };
                    }
                    else if (checkBox.Name == "voc")
                    {
                        checkBox.MouseHover += delegate
                        {
                            CreateToolTip("Газоподібні органічні речовини (поставити відмітку, якщо речовина є газоподібною)", checkBox);
                        };
                    }
                    else if (checkBox.Name == "hydro")
                    {
                        checkBox.MouseHover += delegate
                        {
                            CreateToolTip("Вуглеводнева сполука (поставити мітку, якщо речовина містить в складі та формулі лише «С» - вуглець та «Н» - водень)", checkBox);
                        };
                    }
                }
            }
        }

        private void CreateToolTip(string text, Control control)
        {
            if (tableName == "elements")
                text = string.Format(text, "речовини");
            else if (tableName == "gdk")
                text = string.Format(text, "гдк");
            else if (tableName == "environment")
                text = string.Format(text, "середовища");
            new ToolTip().Show(text, control, 0, control.Height, 2000);
        }

        private void startTutorial_MouseEnter(object sender, EventArgs e)
        {
            startTutorial.Font = new Font(startTutorial.Font, FontStyle.Bold);
        }

        private void startTutorial_MouseLeave(object sender, EventArgs e)
        {
            startTutorial.Font = new Font(startTutorial.Font, FontStyle.Regular);
        }
    }

}
