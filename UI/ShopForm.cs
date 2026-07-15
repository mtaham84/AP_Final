using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AP_Final_Project.UI
{
    public class ShopForm : Form
    {
        private TableLayoutPanel mainLayout = null!;
        private ComboBox cmbCategory = null!;
        private ListBox lstItems = null!;
        private Button btnPurchase = null!;
        private Button btnEquip = null!;
        private Button btnBack = null!;
        private Label lblCoins = null!;

        private List<ShopItem> allItems = new();

        public ShopForm()
        {
            InitializeComponents();
            LoadItems();
            UpdateCoinsDisplay();
        }

        private void InitializeComponents()
        {
            this.Text = "SHOP";
            this.BackColor = Color.FromArgb(20, 20, 35);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(550, 500);
            this.Size = new Size(650, 550);

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(20)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            lblCoins = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Gold,
                TextAlign = ContentAlignment.MiddleCenter
            };

            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(30, 30, 45),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new object[] { "Bullet Style", "Ship Skin", "Background Theme" });
            cmbCategory.SelectedIndex = 0;
            cmbCategory.SelectedIndexChanged += (s, e) => FilterItems();

            lstItems = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(30, 30, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10)
            };
            btnPurchase = new Button { Text = "PURCHASE", Size = new Size(130, 45), BackColor = Color.SteelBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnEquip = new Button { Text = "EQUIP", Size = new Size(130, 45), BackColor = Color.SeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            buttonPanel.Controls.Add(btnPurchase);
            buttonPanel.Controls.Add(btnEquip);

            btnBack = new Button
            {
                Text = "BACK",
                Dock = DockStyle.Fill,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnPurchase.Click += BtnPurchase_Click!;
            btnEquip.Click += BtnEquip_Click!;
            btnBack.Click += (s, e) => this.Close();

            mainLayout.Controls.Add(lblCoins, 0, 0);
            mainLayout.Controls.Add(cmbCategory, 0, 1);
            mainLayout.Controls.Add(lstItems, 0, 2);
            mainLayout.Controls.Add(buttonPanel, 0, 3);
            mainLayout.Controls.Add(btnBack, 0, 4);

            this.Controls.Add(mainLayout);
        }

        private void LoadItems()
        {
            allItems = DatabaseHelper.GetShopItems();
            FilterItems();
        }

        private void FilterItems()
        {
            string category = cmbCategory.SelectedItem?.ToString() ?? "Bullet Style";
            lstItems.Items.Clear();

            List<ShopItem> filtered = allItems.FindAll(i => i.Category == category);

            foreach (var item in filtered)
            {
                string status = item.IsEquipped ? "[Equipped]" : (item.IsPurchased ? "[Purchased]" : $"[Price: {item.Price}]");
                string displayName = item.Name ?? "Unknown";
                lstItems.Items.Add($"{displayName} {status}");
            }
            lstItems.Tag = filtered;
        }

        private void UpdateCoinsDisplay()
        {
            lblCoins.Text = $"YOUR COINS: {DatabaseHelper.GetCoins()}";
        }

        private ShopItem? GetSelectedItem()
        {
            if (lstItems.SelectedIndex == -1) return null;
            var items = lstItems.Tag as List<ShopItem>;
            if (items == null || lstItems.SelectedIndex >= items.Count) return null;
            return items[lstItems.SelectedIndex];
        }

        private void BtnPurchase_Click(object? sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null) { MessageBox.Show("Select an item first."); return; }
            if (item.IsPurchased) { MessageBox.Show("Already purchased!"); return; }

            if (DatabaseHelper.GetCoins() >= item.Price)
            {
                DatabaseHelper.PurchaseItem(item.Id);
                MessageBox.Show($"You purchased {item.Name}!");
                LoadItems();
                UpdateCoinsDisplay();
            }
            else
            {
                MessageBox.Show("Not enough coins!");
            }
        }

        private void BtnEquip_Click(object? sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null) { MessageBox.Show("Select an item first."); return; }
            if (!item.IsPurchased) { MessageBox.Show("You must purchase this item first."); return; }

            DatabaseHelper.EquipItem(item.Id);
            MessageBox.Show($"{item.Name} equipped!");
            LoadItems();
        }
    }
}
