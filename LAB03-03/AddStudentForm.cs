using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LAB03_03
{
    // 1. Định nghĩa Delegate (Theo yêu cầu trang 4)
    public delegate void AddStudentDelegate(string studentID, string fullName, string gender, float score, string faculty);

    public class AddStudentForm : Form
    {
        // Khai báo sự kiện sử dụng Delegate
        public AddStudentDelegate OnAddStudent;

        // Các control
        private TextBox txtID, txtName, txtScore;
        private RadioButton radMale, radFemale;
        private ComboBox cmbFaculty;
        private Button btnAdd, btnExit;

        public AddStudentForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Thêm Sinh Viên";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;

            // --- Label & TextBox: Mã SV ---
            Label lblID = new Label { Text = "Mã SV:", Location = new Point(20, 20), AutoSize = true };
            txtID = new TextBox { Location = new Point(120, 18), Width = 200 };

            // --- Label & TextBox: Tên SV ---
            Label lblName = new Label { Text = "Tên Sinh Viên:", Location = new Point(20, 60), AutoSize = true };
            txtName = new TextBox { Location = new Point(120, 58), Width = 200 };

            // --- Label & RadioButton: Giới tính ---
            Label lblGender = new Label { Text = "Giới tính:", Location = new Point(20, 100), AutoSize = true };
            radMale = new RadioButton { Text = "Nam", Location = new Point(120, 98), AutoSize = true };
            radFemale = new RadioButton { Text = "Nữ", Location = new Point(180, 98), AutoSize = true, Checked = true }; // Mặc định Nữ (Yêu cầu trang 12)

            // --- Label & ComboBox: Khoa ---
            Label lblFaculty = new Label { Text = "Khoa:", Location = new Point(20, 140), AutoSize = true };
            cmbFaculty = new ComboBox { Location = new Point(120, 138), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            // Thêm dữ liệu mẫu (Yêu cầu trang 12)
            cmbFaculty.Items.AddRange(new string[] { "Công nghệ thông tin", "Ngôn ngữ Anh", "Quản trị kinh doanh", "Công nghệ ô tô" });
            cmbFaculty.SelectedIndex = 0; // Mặc định chọn cái đầu

            // --- Label & TextBox: Điểm TB ---
            Label lblScore = new Label { Text = "Điểm TB:", Location = new Point(20, 180), AutoSize = true };
            txtScore = new TextBox { Location = new Point(120, 178), Width = 200 };

            // --- Buttons ---
            btnAdd = new Button { Text = "Thêm / Cập nhật", Location = new Point(80, 240), Width = 110, BackColor = Color.LightGreen };
            btnAdd.Click += BtnAdd_Click;

            btnExit = new Button { Text = "Thoát", Location = new Point(210, 240), Width = 100, BackColor = Color.LightPink };
            btnExit.Click += BtnExit_Click;

            // Add controls
            this.Controls.AddRange(new Control[] { lblID, txtID, lblName, txtName, lblGender, radMale, radFemale, lblFaculty, cmbFaculty, lblScore, txtScore, btnAdd, btnExit });
        }

        // Xử lý nút Thêm/Cập nhật
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput(); // Kiểm tra dữ liệu (Yêu cầu trang 12)

                string id = txtID.Text;
                string name = txtName.Text;
                string gender = radMale.Checked ? "Nam" : "Nữ";
                string faculty = cmbFaculty.SelectedItem.ToString();
                float score = float.Parse(txtScore.Text);

                // Gọi Delegate để truyền dữ liệu về Form cha
                // Dấu ? để kiểm tra null (nếu form cha chưa gán sự kiện thì không lỗi)
                OnAddStudent?.Invoke(id, name, gender, score, faculty);

                // Không đóng form ngay để người dùng có thể nhập tiếp hoặc sửa, chỉ hiện thông báo ở Form cha
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Hàm kiểm tra hợp lệ (Validation - Trang 12)
        private void ValidateInput()
        {
            // 1. Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtScore.Text))
                throw new Exception("Vui lòng nhập đầy đủ thông tin!");

            // 2. Kiểm tra Mã SV: Phải là số và dài 10 ký tự
            if (!Regex.IsMatch(txtID.Text, @"^\d{10}$"))
                throw new Exception("Mã số sinh viên không hợp lệ! (Phải là 10 chữ số)");

            // 3. Kiểm tra Tên: 3-100 ký tự, không chứa số/ký tự đặc biệt
            // \p{L} là ký tự chữ cái unicode (bao gồm tiếng Việt), \s là khoảng trắng
            if (!Regex.IsMatch(txtName.Text, @"^[\p{L}\s]{3,100}$"))
                throw new Exception("Họ tên sinh viên không hợp lệ! (3-100 ký tự chữ)");

            // 4. Kiểm tra Điểm: 0-10, số thực
            if (!float.TryParse(txtScore.Text, out float score) || score < 0 || score > 10)
                throw new Exception("Điểm trung bình phải nằm trong khoảng từ 0 đến 10!");
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}