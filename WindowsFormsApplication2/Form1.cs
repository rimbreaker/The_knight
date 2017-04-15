using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{


   
    public partial class Form1 : Form
    {
        public Boolean gameStarted = false;
        Boolean isFired = false;
        Boolean isLeft = false;
        Boolean isKeyCollected = false;
        Boolean isEditMode = false;
        int mode = 2;
        Point np;
        TableLayoutPanel panel = new TableLayoutPanel();
        Random rnd = new Random();
        Knight wiedzmin = new Knight();
        Bitmap myimage = new Bitmap(@"knight.png");
        Bitmap keyImage = new Bitmap(@"key2.png");
        Bitmap doorImage = new Bitmap(@"closeddoor.png");
        Bitmap doorImage2 = new Bitmap(@"openeddoor.png");
        Tuple<int, int> keyCord;
        Tuple<int, int> doorCord;
        Form3 _splash;
        int current_size = 10;
        public Form1(Form3 splash)
        {
            InitializeComponent();
            _splash = splash;
            this.CenterToScreen();            
            this.FormBorderStyle = FormBorderStyle.FixedSingle;            
            this.FormClosing += OnClosing;
            this.repaintt(10);
            this.grassToolStripMenuItem.Checked = false;
            this.wallToolStripMenuItem.Checked = true;
            this.leftClickToolStripMenuItem.Visible = false;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            isFired = false;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(isFired != true && isEditMode != true)
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (isLeft != true)
                        {
                            isLeft = true;
                            myimage.RotateFlip(RotateFlipType.Rotate180FlipY);
                            wiedzmin.position.Refresh();
                        }
                        if (wiedzmin.xCord-1 >= 0)
                        {
                            if(panel.GetControlFromPosition(wiedzmin.xCord - 1, wiedzmin.yCord).BackColor != Color.Maroon)
                                move(wiedzmin.xCord - 1, wiedzmin.yCord);
                        }
                      
                        isFired = true;
                        break;
                    case Keys.Right:
                        if (isLeft != false)
                        {
                            isLeft = false;
                            myimage.RotateFlip(RotateFlipType.Rotate180FlipY);
                            wiedzmin.position.Refresh();
                        }
                        if (wiedzmin.xCord + 1 < current_size)
                        {
                            if(panel.GetControlFromPosition(wiedzmin.xCord + 1, wiedzmin.yCord).BackColor != Color.Maroon)
                                 move(wiedzmin.xCord + 1, wiedzmin.yCord);
                        }
                        
                        isFired = true;
                        break;
                    case Keys.Up:
                        if (wiedzmin.yCord -1 >= 0)
                        {
                            if(panel.GetControlFromPosition(wiedzmin.xCord, wiedzmin.yCord - 1).BackColor != Color.Maroon)
                             move(wiedzmin.xCord, wiedzmin.yCord-1);
                        }
                        isFired = true;
                        break;
                    case Keys.Down:
                        if (wiedzmin.yCord +1 < current_size)
                        {
                            if(panel.GetControlFromPosition(wiedzmin.xCord, wiedzmin.yCord + 1).BackColor != Color.Maroon )
                                move(wiedzmin.xCord, wiedzmin.yCord +1);
                        }
                        isFired = true;
                        break;
                    case Keys.Space:
                        if(wiedzmin.xCord - 1 >= 0)                        
                            panel.GetControlFromPosition(wiedzmin.xCord - 1, wiedzmin.yCord).BackColor = Color.ForestGreen; 
                        if(wiedzmin.xCord + 1 < current_size)
                            panel.GetControlFromPosition(wiedzmin.xCord + 1, wiedzmin.yCord).BackColor = Color.ForestGreen;
                        if (wiedzmin.yCord - 1 >= 0 )
                            panel.GetControlFromPosition(wiedzmin.xCord, wiedzmin.yCord-1).BackColor = Color.ForestGreen;
                        if (wiedzmin.yCord + 1 < current_size)
                            panel.GetControlFromPosition(wiedzmin.xCord, wiedzmin.yCord +1).BackColor = Color.ForestGreen;
                        break;
                }
        }
        public void repaintt(int size)
        {            
            current_size = size;
            if (gameStarted == true)
            {
                panel.GetControlFromPosition(wiedzmin.xCord, wiedzmin.yCord).Controls.Clear();
                panel.GetControlFromPosition(keyCord.Item1, keyCord.Item2).Controls.Clear();
                panel.GetControlFromPosition(doorCord.Item1, doorCord.Item2).Controls.Clear();
                wiedzmin.position.Controls.Clear();
                for (int x = 0; x < size; x++)
                {                   
                    for (int y = 0; y < size; y++)
                    {
                       panel.GetControlFromPosition(x, y).BackColor = Color.White;
                    }
                }               
            }
            if (gameStarted != true)
            {
                panel.Controls.Clear();
                panel.ColumnCount = size;
                panel.RowCount = size;
                panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                panel.Padding = new Padding(0, 23, 0, 0);
                panel.Dock = DockStyle.Fill;
                for (int x = 0; x < size; x++)
                {
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    for (int y = 0; y < size; y++)
                    {
                        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
                        PictureBox tmpButton = new PictureBox();
                        tmpButton.Dock = DockStyle.Fill;
                        tmpButton.Margin = new Padding(0);
                        tmpButton.BackColor = Color.White;
                        panel.Controls.Add(tmpButton, x, y);
                    }
                }
                this.Controls.Add(panel);
                gameStarted = true;              
                foreach(PictureBox pb in panel.Controls) {
                    pb.MouseClick += this.clickOnSpace;
                }
            }
            if (gameStarted == true)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        int n = rnd.Next();
                        int d = 5;
                        if (x - 1 >= 0 && panel.GetControlFromPosition(x - 1, y).BackColor == Color.Maroon)
                        {
                            d = 2;
                        }                       
                        else if (y - 1 >= 0 && panel.GetControlFromPosition(x, y - 1).BackColor == Color.Maroon)
                        {
                            d = 2;
                        }                        
                        if (n % d == 0)
                            panel.GetControlFromPosition(x, y).BackColor = Color.Maroon;
                        else
                            panel.GetControlFromPosition(x, y).BackColor = Color.ForestGreen;
                    }
                }
                placeDoors();
                placeKey();
                placeKnight();                
                return;
            }                                       
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Close the app?", "Closing",
              MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Cancel the Closing event from closing the form.
                this._splash.Dispose();
                this.Dispose();
                // Call method to save file...
            }
        }
        public void placeKnight()
        {
            int placex = rnd.Next(0, current_size);
            int placeY = rnd.Next(0,current_size);
           
            while (panel.GetControlFromPosition(placex, placeY).BackColor == Color.Maroon || placex == doorCord.Item1 || placeY == doorCord.Item2 || placex == keyCord.Item1 || placeY == keyCord.Item2)
            {
                placex = rnd.Next(0,current_size);
                placeY = rnd.Next(0,current_size);                
            }
            PictureBox knight = new PictureBox();
            knight.BackColor = Color.Transparent;
            
            myimage.MakeTransparent();
            knight.BackgroundImage = myimage;
            knight.BackgroundImageLayout = ImageLayout.Stretch;
            knight.Dock = DockStyle.Fill;
            wiedzmin.position =(PictureBox) panel.GetControlFromPosition(placex, placeY);            
            wiedzmin.position.Controls.Add(knight);
            wiedzmin.xCord = placex;
            wiedzmin.yCord = placeY;
        }
        public void placeKey()
        {
            int placex = rnd.Next(0, current_size);
            int placeY = rnd.Next(0, current_size);
            
            while (panel.GetControlFromPosition(placex, placeY).BackColor == Color.Maroon || placex == doorCord.Item1 || placeY == doorCord.Item2)
            {
                placex = rnd.Next(0, current_size);
                placeY = rnd.Next(0, current_size);
            }
            keyCord = new Tuple<int, int>(placex,placeY);
            PictureBox key = new PictureBox();
            key.BackColor = Color.Transparent;
            keyImage.MakeTransparent();
            key.BackgroundImage = keyImage;
            key.BackgroundImageLayout = ImageLayout.Stretch;
            key.Dock = DockStyle.Fill;
            panel.GetControlFromPosition(placex, placeY).Controls.Add(key);        
        }

        public void placeDoors()
        {
            int placex = rnd.Next(0, current_size);
            int placeY = rnd.Next(0, current_size);

            while (panel.GetControlFromPosition(placex, placeY).BackColor == Color.Maroon)
            {
                placex = rnd.Next(0, current_size);
                placeY = rnd.Next(0, current_size);
            }
            doorCord = new Tuple<int, int>(placex, placeY);
            PictureBox door = new PictureBox();
            door.BackColor = Color.Transparent;
            doorImage.MakeTransparent();
            door.BackgroundImage = doorImage;
            door.BackgroundImageLayout = ImageLayout.Stretch;
            door.Dock = DockStyle.Fill;
            panel.GetControlFromPosition(placex, placeY).Controls.Add(door);
        }
        void move(int x, int y)
        {          
            if (keyCord.Item1 == x && keyCord.Item2 == y)
            {
                isKeyCollected = true;
                panel.GetControlFromPosition(doorCord.Item1, doorCord.Item2).Controls.Clear();
                PictureBox door = new PictureBox();
                door.BackColor = Color.Transparent;
                doorImage2.MakeTransparent();
                door.BackgroundImage = doorImage2;
                door.BackgroundImageLayout = ImageLayout.Stretch;
                door.Dock = DockStyle.Fill;                
                panel.GetControlFromPosition(doorCord.Item1, doorCord.Item2).Controls.Add(door);
            }
            if (doorCord.Item1 == x && doorCord.Item2 == y)
            {
                if (isKeyCollected != true)
                    return;
                else {
                    this.newGameToolStripMenuItem.PerformClick();
                    isKeyCollected = false;
                    return;
                }
            }
            PictureBox knight = new PictureBox();
            knight.BackColor = Color.Transparent;
            myimage.MakeTransparent();
            knight.BackgroundImage = myimage;
            knight.BackgroundImageLayout = ImageLayout.Stretch;
            knight.Dock = DockStyle.Fill;
            wiedzmin.xCord = x;
            wiedzmin.yCord = y;
            wiedzmin.position.Controls.Clear();
            panel.GetControlFromPosition(x, y).Controls.Clear();
            wiedzmin.position = (PictureBox)panel.GetControlFromPosition(x, y);
            wiedzmin.position.Controls.Add(knight);
            
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            Form2 new2 = new Form2(this);
            new2.Show();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.repaintt(current_size);            
        }
        private void editMenu_click(object sender, EventArgs e)
        {
            if (this.isEditMode != true)
            {
                this.editMenu.Text = "Game mode";
                this.leftClickToolStripMenuItem.Visible = true;
                this.isEditMode = true;
                this.menuStrip1.BackColor = Color.Orange;
            }
            else
            {
                this.leftClickToolStripMenuItem.Visible = false;
                this.editMenu.Text = "Edit mode";
                this.isEditMode = false;
                this.menuStrip1.BackColor = Color.LightGray;
            }
        }
        private DialogResult showCloseDialog()
        {
            return MessageBox.Show("Close the app?", "Closing", MessageBoxButtons.YesNo);
        }
        private void OnClosing(Object sender, FormClosingEventArgs args)
        {
            var dialogResult = showCloseDialog();
            if (dialogResult == DialogResult.Yes)
            {               
                this.Dispose();
                this._splash.Dispose();
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void grassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grassToolStripMenuItem.Checked = true;
            this.wallToolStripMenuItem.Checked = false;
            this.mode = 1;
        }

        private void wallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grassToolStripMenuItem.Checked = false;
            this.wallToolStripMenuItem.Checked = true;
            this.mode = 2;
        }
       
        public void clickOnSpace(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isEditMode == true)
            {
                if (mode == 2)
                {
                    if (panel.GetControlFromPosition(panel.GetColumn((PictureBox)sender), panel.GetRow((PictureBox)sender)).BackColor != Color.Maroon)
                        panel.GetControlFromPosition(panel.GetColumn((PictureBox)sender), panel.GetRow((PictureBox)sender)).BackColor = Color.Maroon;
                }
                else
                {
                    if (panel.GetControlFromPosition(panel.GetColumn((PictureBox)sender), panel.GetRow((PictureBox)sender)).BackColor != Color.ForestGreen)
                        panel.GetControlFromPosition(panel.GetColumn((PictureBox)sender), panel.GetRow((PictureBox)sender)).BackColor = Color.ForestGreen;
                }
            }
            else if(isEditMode == true)
            {
                contextMenuStrip1.Show(panel.GetControlFromPosition(panel.GetColumn((PictureBox)sender), panel.GetRow((PictureBox)sender)), e.Location.X,e.Location.Y);
                np = (Point)GetRowColIndex(panel, panel.PointToClient(Cursor.Position));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        Point? GetRowColIndex(TableLayoutPanel tlp, Point point)
        {
            if (point.X > tlp.Width || point.Y > tlp.Height)
                return null;

            int w = tlp.Width;
            int h = tlp.Height;
            int[] widths = tlp.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
                w -= widths[i];
            int col = i + 1;

            int[] heights = tlp.GetRowHeights();
            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
                h -= heights[i];

            int row = i + 1;

            return new Point(col, row);
        }

        private void keyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int placex = np.X;
            int placeY = np.Y;
            if (panel.GetControlFromPosition(placex, placeY).BackColor != Color.Maroon)
            {
                panel.GetControlFromPosition(wiedzmin.xCord, wiedzmin.yCord).Controls.Clear();
                wiedzmin.xCord = placex;
                wiedzmin.yCord = placeY;
                PictureBox knight = new PictureBox();
                knight.BackColor = Color.Transparent;
                myimage.MakeTransparent();
                knight.BackgroundImage = myimage;
                knight.BackgroundImageLayout = ImageLayout.Stretch;
                knight.Dock = DockStyle.Fill;
                wiedzmin.position.Controls.Clear();
                wiedzmin.position = (PictureBox)panel.GetControlFromPosition(placex, placeY);
                panel.GetControlFromPosition(placex, placeY).Controls.Add(knight);
            }
        }

        private void keyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int placex = np.X;
            int placeY = np.Y;
            if (panel.GetControlFromPosition(placex, placeY).BackColor != Color.Maroon)
            {
                panel.GetControlFromPosition(keyCord.Item1, keyCord.Item2).Controls.Clear();
                keyCord = new Tuple<int, int>(placex, placeY);
                PictureBox key = new PictureBox();
                key.BackColor = Color.Transparent;
                keyImage.MakeTransparent();
                key.BackgroundImage = keyImage;
                key.BackgroundImageLayout = ImageLayout.Stretch;
                key.Dock = DockStyle.Fill;
                panel.GetControlFromPosition(placex, placeY).Controls.Add(key);
                if(isKeyCollected == true)
                {
                    isKeyCollected = false;
                    panel.GetControlFromPosition(doorCord.Item1, doorCord.Item2).Controls.Clear();
                    PictureBox door = new PictureBox();
                    door.BackColor = Color.Transparent;
                    doorImage2.MakeTransparent();
                    door.BackgroundImage = doorImage;
                    door.BackgroundImageLayout = ImageLayout.Stretch;
                    door.Dock = DockStyle.Fill;
                    panel.GetControlFromPosition(doorCord.Item1, doorCord.Item2).Controls.Add(door);
                }
            }
        }

        private void doorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int placex = np.X;
            int placeY = np.Y;
            if (panel.GetControlFromPosition(placex, placeY).BackColor != Color.Maroon)
            {
                panel.GetControlFromPosition(doorCord.Item1, doorCord.Item2).Controls.Clear();
                doorCord = new Tuple<int, int>(placex, placeY);
                PictureBox door = new PictureBox();
                door.BackColor = Color.Transparent;
                doorImage.MakeTransparent();
                door.BackgroundImage = doorImage;
                door.BackgroundImageLayout = ImageLayout.Stretch;
                door.Dock = DockStyle.Fill;
                panel.GetControlFromPosition(placex, placeY).Controls.Add(door);
            }
        }
    }
    public class Knight
    {

        public PictureBox position;
        public int xCord;
        public int yCord;
        public Knight()
        {

        }
    }
}
