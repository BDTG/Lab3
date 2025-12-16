using System;
using System.Drawing;
using System.Drawing.Text; // Để lấy danh sách Font hệ thống
using System.IO;
using System.Windows.Forms;

namespace LAB03_02
{
    public partial class MainForm : Form
    {
        // Khai báo các Control
        private MenuStrip menuStrip;
        private ToolStrip toolStrip;
        private RichTextBox richTextBox;

        // Các control trên ToolStrip
        private ToolStripButton btnNew;
        private ToolStripButton btnSave;
        private ToolStripComboBox cmbFonts;
        private ToolStripComboBox cmbSizes;
        private ToolStripButton btnBold;
        private ToolStripButton btnItalic;
        private ToolStripButton btnUnderline;

        // Biến lưu đường dẫn file hiện tại (để xử lý Save)
        private string currentFilePath = null;

        public MainForm()
        {
            InitializeCustomComponents();
            LoadFontAndSizeData(); // Yêu cầu trang 8: Load dữ liệu ban đầu
        }

        private void InitializeCustomComponents()
        {
            // --- 1. Cấu hình Form ---
            this.Text = "Soạn thảo văn bản";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10); // Cải thiện Font chữ

            // --- 2. Tạo MenuStrip (Menu Hệ thống) ---
            menuStrip = new MenuStrip();
            menuStrip.Font = new Font("Segoe UI", 10);
            ToolStripMenuItem systemMenu = new ToolStripMenuItem("Hệ thống");

            // Các menu item con
            ToolStripMenuItem itemNew = new ToolStripMenuItem("Tạo văn bản mới", null, NewDoc_Click);
            itemNew.ShortcutKeys = Keys.Control | Keys.N;

            ToolStripMenuItem itemOpen = new ToolStripMenuItem("Mở tập tin", null, OpenFile_Click);
            itemOpen.ShortcutKeys = Keys.Control | Keys.O;

            ToolStripMenuItem itemSave = new ToolStripMenuItem("Lưu nội dung văn bản", null, SaveFile_Click);
            itemSave.ShortcutKeys = Keys.Control | Keys.S;

            ToolStripMenuItem itemExit = new ToolStripMenuItem("Thoát", null, Exit_Click);

            systemMenu.DropDownItems.AddRange(new ToolStripItem[] { itemNew, itemOpen, itemSave, new ToolStripSeparator(), itemExit });
            menuStrip.Items.Add(systemMenu);

            // --- 3. Tạo ToolStrip (Thanh công cụ định dạng) ---
            toolStrip = new ToolStrip();
            toolStrip.Font = new Font("Segoe UI", 10);

            // Nút New/Save nhanh trên Toolbar
            btnNew = new ToolStripButton(SystemIcons.WinLogo.ToBitmap()); // Dùng icon tạm
            btnNew.Text = "New";
            btnNew.Click += NewDoc_Click;

            btnSave = new ToolStripButton(SystemIcons.Shield.ToBitmap()); // Dùng icon tạm
            btnSave.Text = "Save";
            btnSave.Click += SaveFile_Click;

            // ComboBox Font
            cmbFonts = new ToolStripComboBox();
            cmbFonts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFonts.Size = new Size(150, 25);
            cmbFonts.SelectedIndexChanged += CmbFonts_SelectedIndexChanged;

            // ComboBox Size
            cmbSizes = new ToolStripComboBox();
            cmbSizes.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSizes.Size = new Size(50, 25);
            cmbSizes.SelectedIndexChanged += CmbSizes_SelectedIndexChanged;

            // Các nút định dạng B, I, U
            btnBold = CreateFormatButton("B", FontStyle.Bold);
            btnBold.Click += BtnBold_Click;

            btnItalic = CreateFormatButton("I", FontStyle.Italic);
            btnItalic.Click += BtnItalic_Click;

            btnUnderline = CreateFormatButton("U", FontStyle.Underline);
            btnUnderline.Click += BtnUnderline_Click;

            // Thêm tất cả vào ToolStrip
            toolStrip.Items.AddRange(new ToolStripItem[] {
                btnNew, btnSave,
                new ToolStripSeparator(),
                cmbFonts, cmbSizes,
                new ToolStripSeparator(),
                btnBold, btnItalic, btnUnderline
            });

            // --- 4. Tạo RichTextBox ---
            richTextBox = new RichTextBox();
            richTextBox.Dock = DockStyle.Fill;

            // --- 5. Add controls vào Form ---
            this.Controls.Add(richTextBox); // Fill nằm dưới cùng
            this.Controls.Add(toolStrip);   // Dock Top
            this.Controls.Add(menuStrip);   // Dock Top trên cùng
            this.MainMenuStrip = menuStrip;
        }

        // Helper tạo nút định dạng nhanh
        private ToolStripButton CreateFormatButton(string text, FontStyle style)
        {
            ToolStripButton btn = new ToolStripButton();
            btn.Text = text;
            btn.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btn.Font = new Font("Segoe UI", 9, style);
            return btn;
        }

        // --- YÊU CẦU TRANG 8: Khởi tạo dữ liệu Font/Size ---
        private void LoadFontAndSizeData()
        {
            // 1. Load Font hệ thống
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            foreach (FontFamily font in installedFonts.Families)
            {
                cmbFonts.Items.Add(font.Name);
            }

            // 2. Load Size (8 -> 72)
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in sizes)
            {
                cmbSizes.Items.Add(size);
            }

            // 3. Giá trị mặc định: Segoe UI, 14
            cmbFonts.SelectedItem = "Segoe UI";
            cmbSizes.SelectedItem = 14;

            // Áp dụng font mặc định cho RichTextBox
            ApplyFontToRichText("Segoe UI", 14);
        }

        private void ApplyFontToRichText(string fontName, float size)
        {
            richTextBox.Font = new Font(fontName, size);
        }

        // --- YÊU CẦU TRANG 8: Tạo văn bản mới ---
        private void NewDoc_Click(object sender, EventArgs e)
        {
            // Xóa nội dung
            richTextBox.Clear();

            // Reset về mặc định
            cmbFonts.SelectedItem = "Tahoma";
            cmbSizes.SelectedItem = 14;
            richTextBox.Font = new Font("Tahoma", 14);

            // Reset đường dẫn file
            currentFilePath = null;
        }

        // --- YÊU CẦU TRANG 8: Thoát ---
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // --- YÊU CẦU TRANG 9: Mở tập tin ---
        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text Files|*.txt|Rich Text Format|*.rtf"; // *.txt hoặc *.rtf

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = dlg.FileName;
                string extension = Path.GetExtension(currentFilePath).ToLower();

                try
                {
                    if (extension == ".txt")
                    {
                        // Đọc file UTF-8
                        using (StreamReader sr = new StreamReader(currentFilePath, System.Text.Encoding.UTF8))
                        {
                            richTextBox.Text = sr.ReadToEnd();
                        }
                    }
                    else if (extension == ".rtf")
                    {
                        richTextBox.LoadFile(currentFilePath);
                    }
                    MessageBox.Show("Mở tập tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi định dạng tệp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- YÊU CẦU TRANG 9: Lưu nội dung ---
        private void SaveFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                // Trường hợp 1: Văn bản mới chưa lưu lần nào -> SaveFileDialog
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Rich Text Format|*.rtf"; // Mặc định lưu .rtf

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = dlg.FileName;
                    richTextBox.SaveFile(currentFilePath);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo");
                }
            }
            else
            {
                // Trường hợp 2: Văn bản đã mở trước đó -> Lưu đè
                try
                {
                    // Lưu ý: RichTextBox save mặc định thường là RTF, 
                    // nếu muốn save lại file .txt cần check đuôi file
                    string ext = Path.GetExtension(currentFilePath).ToLower();
                    if (ext == ".txt")
                    {
                        richTextBox.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                    }
                    else
                    {
                        richTextBox.SaveFile(currentFilePath);
                    }

                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                }
            }
        }

        // --- YÊU CẦU TRANG 10: Định dạng (Đậm, Nghiêng, Gạch dưới) ---
        // Hàm dùng chung để đổi Style
        private void ToggleFontStyle(FontStyle style)
        {
            if (richTextBox.SelectionFont != null)
            {
                Font currentFont = richTextBox.SelectionFont;
                FontStyle newStyle;

                // Kiểm tra xem style đó đã có chưa để bật/tắt (XOR logic hoặc check contains)
                if (currentFont.Style.HasFlag(style))
                {
                    newStyle = currentFont.Style & ~style; // Xóa style
                }
                else
                {
                    newStyle = currentFont.Style | style; // Thêm style
                }

                richTextBox.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vùng văn bản có cùng định dạng Font để thực hiện!", "Thông báo");
            }
        }

        private void BtnBold_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Bold);
        }

        private void BtnItalic_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void BtnUnderline_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Underline);
        }

        // --- YÊU CẦU TRANG 10 (Code tham khảo): Đổi Font/Size từ ComboBox ---
        private void CmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFonts.SelectedItem == null) return;
            ChangeFontAttr(cmbFonts.SelectedItem.ToString(), -1);
        }

        private void CmbSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSizes.SelectedItem == null) return;
            float newSize = float.Parse(cmbSizes.SelectedItem.ToString());
            ChangeFontAttr(null, newSize);
        }

        // Hàm hỗ trợ đổi Font hoặc Size cho vùng chọn
        private void ChangeFontAttr(string fontName, float size)
        {
            if (richTextBox.SelectionFont != null)
            {
                Font currentFont = richTextBox.SelectionFont;

                // Giữ nguyên cái nào không thay đổi
                string newName = (fontName == null) ? currentFont.Name : fontName;
                float newSize = (size == -1) ? currentFont.Size : size;

                richTextBox.SelectionFont = new Font(newName, newSize, currentFont.Style);
            }
        }
    }
}