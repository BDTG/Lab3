using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LAB03_03
{
    public partial class StudentInfoForm : Form
    {
        private ListView listView;
        private MenuStrip menuStrip;
        private ToolStrip toolStrip;
        private ToolStripLabel lblSearch;
        private ToolStripTextBox txtSearch;
        private ToolStripButton btnStats;

        private ToolStripStatusLabel lblMaleCount;
        private ToolStripStatusLabel lblFemaleCount;
        private StatusStrip statusStrip; // Thêm StatusStrip dưới cùng

        // Biến đếm số thứ tự
        private int sttCounter = 1;

        public StudentInfoForm()
        {
            InitializeComponent();

            // --- GỌI HÀM TẠO DỮ LIỆU MẪU Ở ĐÂY ---
            AddSampleData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Sinh Viên";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            // --- 1. Tạo MenuStrip ---
            menuStrip = new MenuStrip();
            menuStrip.Font = new Font("Segoe UI", 10);
            ToolStripMenuItem funcMenu = new ToolStripMenuItem("Chức năng");

            ToolStripMenuItem addItem = new ToolStripMenuItem("Thêm sinh viên", null, AddItem_Click);
            addItem.ShortcutKeys = Keys.Control | Keys.N;

            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Xóa", null, DeleteItem_Click);
            deleteItem.ShortcutKeys = Keys.Control | Keys.D;

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Thoát", null, ExitItem_Click);

            funcMenu.DropDownItems.Add(addItem);
            funcMenu.DropDownItems.Add(deleteItem); // Thêm menu Xóa
            funcMenu.DropDownItems.Add(exitItem);
            menuStrip.Items.Add(funcMenu);

            // --- 2. Tạo Toolbar ---
            toolStrip = new ToolStrip();
            toolStrip.Font = new Font("Segoe UI", 10);
            lblSearch = new ToolStripLabel("Tìm kiếm tên:");
            txtSearch = new ToolStripTextBox();
            txtSearch.Size = new Size(250, 25);
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.TextChanged += TxtSearch_TextChanged;

            ToolStripButton btnDelete = new ToolStripButton("Xóa", SystemIcons.Error.ToBitmap(), DeleteItem_Click);
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

            btnStats = new ToolStripButton("Thống kê Xếp loại", null, BtnStats_Click);
            btnStats.Image = SystemIcons.Information.ToBitmap();
            btnStats.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

            // Thêm nút Xóa vào Toolbar luôn cho tiện
            toolStrip.Items.AddRange(new ToolStripItem[] { lblSearch, txtSearch, new ToolStripSeparator(), btnDelete, new ToolStripSeparator(), btnStats });

            // --- 3. Tạo ListView ---
            listView = new ListView();
            listView.Dock = DockStyle.Fill;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Font = new Font("Segoe UI", 10);

            // --- CẬP NHẬT KÍCH THƯỚC CỘT CHO ĐẸP ---
            listView.Columns.Add("STT", 60);
            listView.Columns.Add("Mã SV", 120);
            listView.Columns.Add("Tên Sinh Viên", 250);
            listView.Columns.Add("Giới Tính", 100);
            listView.Columns.Add("Điểm TB", 100);
            listView.Columns.Add("Khoa", 250);

            // --- 4. Tạo StatusStrip cho bộ đếm Nam/Nữ ---
            statusStrip = new StatusStrip();
            statusStrip.Font = new Font("Segoe UI", 9);
            lblMaleCount = new ToolStripStatusLabel("Tổng SV Nam: 0");
            lblFemaleCount = new ToolStripStatusLabel("Tổng SV Nữ: 0");

            // Thêm Spacer hoặc căn chỉnh nếu thích, ở đây chỉ add đơn giản
            statusStrip.Items.AddRange(new ToolStripItem[] { lblMaleCount, new ToolStripSeparator(), lblFemaleCount });


            // --- Add controls ---
            this.Controls.Add(listView);
            this.Controls.Add(statusStrip); // Add StatusStrip
            this.Controls.Add(toolStrip);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        // --- HÀM TẠO DỮ LIỆU MẪU ---
        private void AddSampleData()
        {
            // Tạo sinh viên 1: Giỏi
            ReceiveStudentData("1111111111", "Nguyễn Văn An", "Nam", 8.5f, "Công nghệ thông tin");

            // Tạo sinh viên 2: Xuất sắc
            ReceiveStudentData("2222222222", "Trần Thị Bích", "Nữ", 9.5f, "Ngôn ngữ Anh");

            // Tạo sinh viên 3: Trung bình
            ReceiveStudentData("3333333333", "Lê Hoàng Nam", "Nam", 6.5f, "Công nghệ ô tô");

            // --- THÊM DỮ LIỆU MẪU PHONG PHÚ HƠN ---
            ReceiveStudentData("1029384756", "Phạm Minh Tuấn", "Nam", 7.2f, "Quản trị kinh doanh");
            ReceiveStudentData("9876543210", "Hoàng Thị Lan", "Nữ", 5.8f, "Kế toán");
            ReceiveStudentData("5647382910", "Vũ Đức Thắng", "Nam", 3.5f, "Cơ khí");
            ReceiveStudentData("1234567890", "Đặng Ngọc Mai", "Nữ", 9.0f, "Công nghệ thông tin");
            ReceiveStudentData("2345678901", "Bùi Văn Long", "Nam", 4.5f, "Xây dựng");
            ReceiveStudentData("3456789012", "Ngô Thu Hà", "Nữ", 8.0f, "Ngôn ngữ Anh");
            ReceiveStudentData("4567890123", "Lý Thanh Tùng", "Nam", 9.8f, "Công nghệ thông tin");
            ReceiveStudentData("5678901234", "Trịnh Kim Oanh", "Nữ", 6.9f, "Du lịch");
            ReceiveStudentData("6789012345", "Đỗ Văn Hùng", "Nam", 7.5f, "Logistics");
            ReceiveStudentData("7890123456", "Phan Thị Tuyết", "Nữ", 8.2f, "Ngôn ngữ Trung");
            ReceiveStudentData("8901234567", "Hồ Quang Hiếu", "Nam", 5.0f, "Điện - Điện tử");
        }

        // --- Xử lý sự kiện Menu Thêm ---
        private void AddItem_Click(object sender, EventArgs e)
        {
            AddStudentForm frm = new AddStudentForm();
            frm.OnAddStudent = this.ReceiveStudentData;
            frm.ShowDialog();
        }

        // --- Xử lý sự kiện Xóa ---
        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa sinh viên đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    // Xóa các dòng đang chọn
                    foreach (ListViewItem item in listView.SelectedItems)
                    {
                        listView.Items.Remove(item);
                    }
                    UpdateStudentCounts(); // Cập nhật lại số lượng
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- HÀM CẬP NHẬT SỐ LƯỢNG SINH VIÊN ---
        private void UpdateStudentCounts()
        {
            int maleCount = 0;
            int femaleCount = 0;

            foreach (ListViewItem item in listView.Items)
            {
                if (item.SubItems[3].Text == "Nam") maleCount++;
                else if (item.SubItems[3].Text == "Nữ") femaleCount++;
            }

            lblMaleCount.Text = $"Tổng SV Nam: {maleCount}";
            lblFemaleCount.Text = $"Tổng SV Nữ: {femaleCount}";
        }

        // --- HÀM NHẬN DỮ LIỆU (Được tái sử dụng để thêm data mẫu) ---
        private void ReceiveStudentData(string id, string name, string gender, float score, string faculty)
        {
            // Kiểm tra trùng mã
            ListViewItem existingItem = null;
            foreach (ListViewItem item in listView.Items)
            {
                if (item.SubItems[1].Text == id)
                {
                    existingItem = item;
                    break;
                }
            }

            if (existingItem != null)
            {
                // Cập nhật
                existingItem.SubItems[2].Text = name;
                existingItem.SubItems[3].Text = gender;
                existingItem.SubItems[4].Text = score.ToString();
                existingItem.SubItems[5].Text = faculty;

                // Chỉ hiện thông báo khi Form đã hiển thị (tránh hiện 3 popup khi vừa mở app)
                if (this.Visible) MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo");
            }
            else
            {
                // Thêm mới
                ListViewItem newItem = new ListViewItem(sttCounter.ToString());
                newItem.SubItems.Add(id);
                newItem.SubItems.Add(name);
                newItem.SubItems.Add(gender);
                newItem.SubItems.Add(score.ToString());
                newItem.SubItems.Add(faculty);

                listView.Items.Add(newItem);
                sttCounter++;

                // Chỉ hiện thông báo khi Form đã hiển thị
                if (this.Visible) MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo");
            }

            // Cập nhật số lượng
            UpdateStudentCounts();
        }

        // --- Chức năng Tìm kiếm ---
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            // Reset selection
            foreach (ListViewItem item in listView.Items) item.Selected = false;

            foreach (ListViewItem item in listView.Items)
            {
                if (item.SubItems[2].Text.ToLower().Contains(keyword))
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    return; // Chọn người đầu tiên tìm thấy
                }
            }
        }

        // --- Chức năng Thống kê ---
        private void BtnStats_Click(object sender, EventArgs e)
        {
            int xuatSac = 0, gioi = 0, kha = 0, trungBinh = 0, yeu = 0, kem = 0;

            foreach (ListViewItem item in listView.Items)
            {
                float score = float.Parse(item.SubItems[4].Text);

                if (score >= 9) xuatSac++;
                else if (score >= 8) gioi++;
                else if (score >= 7) kha++;
                else if (score >= 5) trungBinh++;
                else if (score >= 4) yeu++;
                else kem++;
            }

            string msg = $"Thống kê xếp loại sinh viên:\n\n" +
                         $"- Xuất sắc (9-10): {xuatSac}\n" +
                         $"- Giỏi (8-9): {gioi}\n" +
                         $"- Khá (7-8): {kha}\n" +
                         $"- Trung bình (5-7): {trungBinh}\n" +
                         $"- Yếu (4-5): {yeu}\n" +
                         $"- Kém (<4): {kem}";

            MessageBox.Show(msg, "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}