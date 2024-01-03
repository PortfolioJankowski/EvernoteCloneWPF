using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.Model
{
    public class Notebook : HasId
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public  string Name { get; set; }   
    }
}
