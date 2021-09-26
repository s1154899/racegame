using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track()
        {
            Sections = new LinkedList<Section>();
        }

        public Track(String name, SectionTypes[] sections) {
            this.Name = name;
            Sections = _arrayToLinkedList(sections);
        }

        private LinkedList<Section> _arrayToLinkedList(SectionTypes[] sections)
        {

            LinkedList<Section> returnValue =  new LinkedList<Section>();

            foreach (var section in sections)
            {
                returnValue.AddLast(new Section(section));
            }

            return returnValue;
        }
    }
}
