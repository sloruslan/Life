using Life.Clients.WinForms.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Clients.WinForms.ViewModel
{
    public class CellsViewModel
    {
        private int _width;
        private int _height;
        private int _sizeCell;
        private PictureBox _pictureBox;
        private Image _image;
        private Graphics _graphics;

        public CellsViewModel(PictureBox pictureBox, int width, int height, int sizeCell)
        {
            _width = width;
            _height = height;
            _sizeCell = sizeCell;
            _pictureBox = pictureBox;
            _image = new Bitmap(width * sizeCell, height * sizeCell);
            _graphics = Graphics.FromImage(_image);
            _pictureBox.Image = _image;
        }

        public void RefreshImage(CellStateField cells)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _graphics.FillRectangle(cells[x, y], x * _sizeCell, y * _sizeCell, _sizeCell, _sizeCell);
                }
            }
            
            _pictureBox.Refresh();  
        }
    }
}
