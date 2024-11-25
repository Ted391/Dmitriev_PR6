using Dmitriev_PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dmitriev_PZ2
{
    internal class Helper
    {
        private static ProgMod_PZ4Entities _context;
        public static ProgMod_PZ4Entities GetContext()
        {
            if (_context == null)
            {
                _context = new ProgMod_PZ4Entities();
            }
            return _context;
        }
    }
}
