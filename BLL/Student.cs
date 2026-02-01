using StudentDataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StudentBusinessLogic
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public StudentDTO SDTO { get { return (new StudentDTO(this.ID, this.Name, this.Age, this.Grade)); } }

      public Student(StudentDTO studentDTO, enMode mode = enMode.AddNew)
        {
            this.ID = studentDTO.Id;
            this.Name = studentDTO.Name;
            this.Age = studentDTO.Age;
            this.Grade = studentDTO.Grade;
            Mode = mode;
        }


        private bool AddNewStudent()
        {
            this.ID = clsStudentData.AddStudent(SDTO);
            return this.ID != -1;
        }

        private  bool UpdateStudent()
        {
            return clsStudentData.UpdateStudent(SDTO);  
        }

        static public List<StudentDTO> GetPassedStudents()
        {
            return clsStudentData.GetPassedStudents();
        }

        static public List<StudentDTO> GetAllStudent()
        {
            return clsStudentData.GetAllStudents();
        }


        static public  double GetAverageGrade()
        {
            return clsStudentData.GetAverageGrade();
        }


        static public  bool DeleteStudent(int ID) {
            return clsStudentData.DeleteStudent(ID);
        }

        public static Student Find(int ID)
        {
            StudentDTO SDTO = clsStudentData.GetStudentById(ID);

            if(SDTO == null)
                return null;

            return new Student(SDTO, enMode.Update);
          
        }


        public bool Save()
        {
            switch (Mode)
            {

                case enMode.AddNew:
                    if (AddNewStudent())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return UpdateStudent();

            }
            return false;

        }


    }
}
