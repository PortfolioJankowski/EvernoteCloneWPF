using EvernoteCloneWPF.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.ViewModel.Commands
{
    public class NewNoteCommand
    {
        public NotesVM VM { get; set; }

        public NewNoteCommand(NotesVM vm)
        {
            VM = vm;
        }
    }
}
