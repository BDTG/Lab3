using System;
using System.Drawing;
using System.Windows.Forms;
using AxWMPLib; // Thư viện Windows Media Player (Interop)
using WMPLib;   // Thư viện Windows Media Player (Core)

namespace LAB03_01
{
    // Đảm bảo class kế thừa từ Form
    public partial class Form1 : Form
    {
        // Khai báo các biến thành viên
        private AxWindowsMediaPlayer wmpPlayer;
        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private Timer timer;

        // Biến quản lý tài nguyên (thay thế cho Designer)
        private System.ComponentModel.IContainer components = null;

        public Form1()
        {
            // Gọi hàm khởi tạo giao diện do mình tự viết
            InitializeCustomComponents();
        }

        // Hàm thiết lập giao diện (Thay thế InitializeComponent)
        private void InitializeCustomComponents()
        {
            this.components = new System.ComponentModel.Container();

            // 1. Cấu hình Form chính
            this.Text = "Chương trình Windows Media Player";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10); // Cải thiện Font chữ

            // 2. Tạo MenuStrip (Thanh menu trên cùng)
            menuStrip = new MenuStrip();
            menuStrip.Font = new Font("Segoe UI", 10); // Cải thiện Font menu

            // Menu File
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem openItem = new ToolStripMenuItem("Open", null, OpenFile_Click);
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit", null, Exit_Click);

            // Phím tắt Ctrl+O cho Open
            openItem.ShortcutKeys = Keys.Control | Keys.O;

            fileMenu.DropDownItems.Add(openItem);
            fileMenu.DropDownItems.Add(exitItem);

            // Menu Thông tin
            ToolStripMenuItem infoMenu = new ToolStripMenuItem("Thông tin");

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(infoMenu);

            // 3. Tạo StatusStrip (Thanh trạng thái dưới cùng)
            statusStrip = new StatusStrip();
            statusStrip.Font = new Font("Segoe UI", 9); // Font status nhỏ hơn chút
            lblStatus = new ToolStripStatusLabel();
            lblStatus.Text = "Ready";
            statusStrip.Items.Add(lblStatus);

            // 4. Tạo Timer (Cập nhật giờ)
            timer = new Timer(this.components);
            timer.Interval = 1000; // 1 giây
            timer.Tick += Timer_Tick;
            timer.Enabled = true;

            // 5. Tạo Windows Media Player Control
            try
            {
                wmpPlayer = new AxWindowsMediaPlayer();
                // Bắt đầu quá trình khởi tạo control COM
                ((System.ComponentModel.ISupportInitialize)(wmpPlayer)).BeginInit();

                wmpPlayer.Dock = DockStyle.Fill; // Lấp đầy màn hình
                wmpPlayer.Enabled = true;

                // Thêm các control vào Form
                // Lưu ý: Add WMP trước để nó nằm dưới, sau đó đến các thanh menu/status
                this.Controls.Add(wmpPlayer);
                this.Controls.Add(statusStrip);
                this.Controls.Add(menuStrip);

                // Kết thúc quá trình khởi tạo control COM
                ((System.ComponentModel.ISupportInitialize)(wmpPlayer)).EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo WMP (Có thể thiếu Reference): " + ex.Message);
            }
        }

        // --- CÁC SỰ KIỆN (EVENTS) ---

        // Sự kiện Timer: Cập nhật ngày giờ mỗi giây
        private void Timer_Tick(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            lblStatus.Text = string.Format("Hôm nay là ngày: {0} - Bây giờ là {1}", date, time);
        }

        // Sự kiện chọn Open: Mở file video
        private void OpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Bộ lọc file theo yêu cầu đề bài
                openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.wmv|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    wmpPlayer.URL = openFileDialog.FileName;
                }
            }
        }

        // Sự kiện chọn Exit: Thoát chương trình
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // --- DỌN DẸP BỘ NHỚ (DISPOSE) ---
        // Hàm này thay thế hàm trong file Designer.cs đã bị xóa
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                if (wmpPlayer != null) wmpPlayer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}