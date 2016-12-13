using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _enrollmentDate;

    public Student(string Name, DateTime EnrollmentDate, int Id=0)
    {
      _id = Id;
      _name = Name;
      _enrollmentDate = EnrollmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.GetId() == newStudent.GetId());
        bool nameEquality = (this.GetName() == newStudent.GetName());
        bool enrollmentDateEquality = (this.GetEnrollmentDate() == newStudent.GetEnrollmentDate());

        return (idEquality && nameEquality && enrollmentDateEquality);
      }
    }

    public override int GetHashCode()
    {
      return _name.GetHashCode();
    }

    public static List<Student> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      List<Student> allStudents = new List<Student> {};
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentEnrollmentDate = rdr.GetDateTime(2);
        allStudents.Add(new Student(studentName, studentEnrollmentDate, studentId));
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allStudents;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students(name, enrollment) OUTPUT INSERTED.id VALUES (@StudentName, @StudentEnrollmentDate)", conn);
      cmd.Parameters.AddWithValue("@StudentName", this.GetName());
      cmd.Parameters.AddWithValue("@StudentEnrollmentDate", this.GetEnrollmentDate());
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Student Find(int targetId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id= @Id;", conn);
      cmd.Parameters.AddWithValue("@Id", targetId);

      SqlDataReader rdr = cmd.ExecuteReader();

      int studentId = 0;
      string studentName = null;
      DateTime studentEnrollmentDate = new DateTime();

      while (rdr.Read())
      {
        studentId = rdr.GetInt32(0);
        studentName = rdr.GetString(1);
        studentEnrollmentDate = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(studentName, studentEnrollmentDate, studentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

    public void UpdateName(string changeValue)
    {
      this.SetName(changeValue);

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE students SET name = @Input WHERE id = @Id;", conn);

      cmd.Parameters.AddWithValue("@Input", changeValue);
      cmd.Parameters.AddWithValue("@Id", this.GetId());

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void UpdateEnrollmentDate(DateTime changeDate)
    {
      this.SetEnrollmentDate(changeDate);

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE students SET enrollment = @Input WHERE id = @Id;", conn);

      cmd.Parameters.AddWithValue("@Input", changeDate);
      cmd.Parameters.AddWithValue("@Id", this.GetId());

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id=@StudentId;", conn);
      cmd.Parameters.AddWithValue("@StudentId", this.GetId());

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }
    public void SetEnrollmentDate(DateTime newEnrollmentDate)
    {
      _enrollmentDate = newEnrollmentDate;
    }
  }
}
