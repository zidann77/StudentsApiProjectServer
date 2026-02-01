using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDataAccess
{
    public class StudentDTO
    {
        public StudentDTO(int id, string name, int age, int grade)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }
    public class clsStudentData
    {
        static string _connectionString = "Server=localhost;Database=StudentsDB;User Id= ;Password= ;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        // Get All Student 

        public static List<StudentDTO> GetAllStudents()
        {
            var Students = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Students.Add(new StudentDTO(reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))));
                        }
                    }
                }
            }

            return Students;
        }

        // GetPasssedStudent 

        public static List<StudentDTO> GetPassedStudents()
        {
            var StudentsList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                            ));
                        }
                    }
                }


                return StudentsList;
            }

        }

        // getAverageStudent 

        public static double GetAverageGrade()
        {
            double averageGrade = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        averageGrade = Convert.ToDouble(result);
                    }
                    else
                        averageGrade = 0;

                }
            }
            return averageGrade;
        }

        // getStudentBy ID

        public static StudentDTO GetStudentById(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetStudentById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        // Add


        public static int AddStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_AddStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }


        // Update 

        public static bool UpdateStudent(StudentDTO studentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentId", studentDTO.Id);
                command.Parameters.AddWithValue("@Name", studentDTO.Name);
                command.Parameters.AddWithValue("@Age", studentDTO.Age);
                command.Parameters.AddWithValue("@Grade", studentDTO.Grade);

               
                command.Parameters.Add("@ReturnValue", SqlDbType.Int)
                       .Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                int rowsAffected = (int)command.Parameters["@ReturnValue"].Value;
                return rowsAffected == 1;
            }
        }



        // delete


        public static bool DeleteStudent(int studentId)
        {

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_DeleteStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);


            }
        }

    }
}
