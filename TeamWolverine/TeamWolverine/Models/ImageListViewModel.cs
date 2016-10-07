using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWolverine.Models
{
public class ImageListViewModel
    {
        public IEnumerable<ImageModel>  ImageList { get; set; }

        public int Total { get; set; }
    }
}
