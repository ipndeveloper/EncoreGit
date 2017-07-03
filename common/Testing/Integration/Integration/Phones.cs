using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class Phones
    {
        List<Phone> _phones = new List<Phone>();

        public Phones(Phone phone)
        {
            AddPhone(phone);
        }

        public int Count
        {
            get { return _phones.Count; }
        }

        public List<Phone> GetAllPhones()
        {
            return _phones;
        }

        public int AddPhone(Phone phone)
        {
            int index = _phones.Count;
            _phones.Add(phone);
            return index;
        }

        public Phone GetPhone(int index)
        {
            Phone phone;            
            phone = _phones[index];
            return phone;
        }
    }
}
