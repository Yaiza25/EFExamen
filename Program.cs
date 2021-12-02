using System.Reflection.Emit;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static System.Console;


public class InstitutoContext : DbContext
{
    public DbSet<Alumno> Alumnos { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }

    public string connString { get; private set; }

    public InstitutoContext()
    {
        var database = "EF04Yaiza"; // "EF{XX}Nombre" => EF00Santi
        connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database={database};User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Matricula>().HasIndex(m => new
        {
            m.AlumnoId,
            m.ModuloId
        }).IsUnique();
    }

}
public class Alumno
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AlumnoId { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public decimal Efectivo { get; set; }
    public string Pelo { get; set; }

    public List<Matricula> Matriculaciones { get; } = new List<Matricula>(); 

    public override string ToString() => $"{AlumnoId}: {Nombre}, edad {Edad}, tipo pelo {Pelo} - Efectivo: {Efectivo}";
}
public class Modulo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ModuloId { get; set; }
    public string Titulo { get; set; }
    public int Creditos { get; set; }
    public int Curso { get; set; }

    public List<Matricula> Matriculaciones { get; } = new List<Matricula>(); 

    public override string ToString() => $"{ModuloId}: {Titulo}, creditos {Creditos}, curso {Curso}";

}
public class Matricula
{
    [Key]
    public int MatriculaId { get; set; }
    public int AlumnoId { get; set; }
    public int ModuloId { get; set; }

    public Alumno Alumno { get; set; }
    public Modulo Modulo { get; set; } 

    public override string ToString() => $"{MatriculaId}: Alumno {AlumnoId} - Modulo {ModuloId}";
}

class Program
{
    static void GenerarDatos()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar todo

            WriteLine("Borrar datos");

            db.Alumnos.RemoveRange(db.Alumnos);
            db.Matriculas.RemoveRange(db.Matriculas);
            db.Modulos.RemoveRange(db.Modulos);

            db.SaveChanges();

            // Añadir Alumnos

            WriteLine("Insertar Alumnos");

            db.Alumnos.Add(new Alumno { AlumnoId = 1, Nombre = "Yaiza", Edad = 18, Efectivo = 12.5M, Pelo = "Rubio" });
            db.Alumnos.Add(new Alumno { AlumnoId = 2, Nombre = "Paula", Edad = 17, Efectivo = 9.6M, Pelo = "Moreno" });
            db.Alumnos.Add(new Alumno { AlumnoId = 3, Nombre = "Maider", Edad = 20, Efectivo = 11.9M, Pelo = "Castaño" });
            db.Alumnos.Add(new Alumno { AlumnoId = 4, Nombre = "Jennifer", Edad = 27, Efectivo = 18.2M, Pelo = "Morena" });
            db.Alumnos.Add(new Alumno { AlumnoId = 5, Nombre = "Pedro", Edad = 21, Efectivo = 15.0M, Pelo = "Rubio" });
            db.Alumnos.Add(new Alumno { AlumnoId = 6, Nombre = "Juan", Edad = 19, Efectivo = 5.5M, Pelo = "Rubio" });
            db.Alumnos.Add(new Alumno { AlumnoId = 7, Nombre = "Lucas", Edad = 24, Efectivo = 17.1M, Pelo = "Castaño" });

            db.SaveChanges();

            // Añadir Módulos
            
            WriteLine("Insertar Módulos");

            db.Modulos.Add(new Modulo { ModuloId = 1, Titulo = "Matematicas", Creditos = 2 , Curso = 1 });
            db.Modulos.Add(new Modulo { ModuloId = 2, Titulo = "Lengua", Creditos = 3 , Curso =  2 });
            db.Modulos.Add(new Modulo { ModuloId = 3, Titulo = "Euskera", Creditos = 5 , Curso =  2 });
            db.Modulos.Add(new Modulo { ModuloId = 4, Titulo = "Castellano", Creditos = 1 , Curso = 2 });
            db.Modulos.Add(new Modulo { ModuloId = 5, Titulo = "Ingles", Creditos = 2 , Curso =  1 });
            db.Modulos.Add(new Modulo { ModuloId = 6, Titulo = "Historia", Creditos = 4 , Curso =  1 });
            db.Modulos.Add(new Modulo { ModuloId = 7, Titulo = "Biologia", Creditos = 2 , Curso = 2 });
            db.Modulos.Add(new Modulo { ModuloId = 8, Titulo = "Fisica", Creditos = 5 , Curso = 1 });
            db.Modulos.Add(new Modulo { ModuloId = 9, Titulo = "Quimica", Creditos = 3 , Curso = 2 });
            db.Modulos.Add(new Modulo { ModuloId = 10, Titulo = "Informatica", Creditos = 1 , Curso = 2 });

            db.SaveChanges();

            // Matricular Alumnos en Módulos

            WriteLine("Matricular Alumnos en Módulos");

            foreach (var modulo in db.Modulos)
            {

                foreach (var alumno in db.Alumnos)
                {
                    db.Matriculas.Add(new Matricula { Alumno = alumno, Modulo = modulo });
                }
                
            }

            db.SaveChanges();

        }
    }

    static void BorrarMatriculaciones()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar las matriculas de AlumnoId multiplo de 3 y ModuloId Multiplo de 2

            WriteLine("Borrar las matriculas de AlumnoId multiplo de 3 y ModuloId Multiplo de 2");

            foreach (var matricula in db.Matriculas)
            {
                if (matricula.AlumnoId % 3 == 0 && matricula.ModuloId % 2 == 0) db.Matriculas.Remove(matricula);
            };

            db.SaveChanges();

            // Borrar las matriculas de AlumnoId multiplo de 2 y ModuloId Multiplo de 5

            WriteLine("Borrar las matriculas de AlumnoId multiplo de 2 y ModuloId Multiplo de 5");

            foreach (var matricula in db.Matriculas)
            {
                if (matricula.AlumnoId % 2 == 0 && matricula.ModuloId % 5 == 0) db.Matriculas.Remove(matricula);
            };

            db.SaveChanges();

        }
    }
    static void RealizarQuery()
    {
        using (var db = new InstitutoContext())
        {
            // Filtering

            var query1 = db.Matriculas.Where(o => o.ModuloId == 1);

            // Return Anonymous Type

            var query2 = db.Alumnos.Select(o => new {
                AlumnoID = o.AlumnoId,
                Nombre = o.Nombre
            });

            // Ordering

            var query3 = db.Modulos.OrderBy(o => o.ModuloId);
            var query4 = db.Modulos.OrderByDescending(o => o.ModuloId);
            var query5 = db.Modulos.OrderBy(o => o.ModuloId).ThenByDescending(o => o.Titulo);

            // Joining

            var query6 = db.Alumnos.Join(db.Matriculas, alu => alu.AlumnoId, mat => mat.AlumnoId, (alu, mat) => new {
                alu.AlumnoId,
                alu.Nombre,
                mat.MatriculaId
            });

            // Grouping

            var query7 = db.Matriculas.GroupBy(o => o.AlumnoId).Select(g => new {
                AlumnoId = g.Key,
                TotalModulos = g.Count()
            });

            // Paging (using Skip & Take)

            var query8 = db.Alumnos.Where(o => o.AlumnoId < 5).Take(3);
            var query9 = db.Alumnos.Where(o => o.AlumnoId < 5).Skip(1);

            // Element Operators (Single, Last, First, ElementAt, Defaults)

            var query10 = db.Alumnos.Single(c => c.AlumnoId == 1);
            var query11 = db.Alumnos.SingleOrDefault(c => c.AlumnoId == 4);
            var query12 = db.Alumnos.Where(c => c.AlumnoId == 7).DefaultIfEmpty().Single();
            var query13 = db.Alumnos.Where(o => o.AlumnoId < 5).OrderBy(o => o.Nombre).Last();
            var query14 = db.Matriculas.Where(c => c.MatriculaId == 3).Select(o => o.MatriculaId).SingleOrDefault();

            // Conversions => ToArray

            var query15 = (from c in db.Alumnos select c.Nombre).ToArray();

            // Conversions => ToDictionary

            Dictionary<int, Alumno> query16 = db.Alumnos.ToDictionary(c => c.AlumnoId);

            // Dictionary<string, decimal> query17 = (from oc in (
            //         from o in db.Alumnos
            //         join c in db.Matriculas on o.AlumnoId equals c.AlumnoId
            //         select new { o.Pelo, o.Efectivo }
            //     )
            //     group oc by oc.Pelo into g
            //     select g).ToDictionary(g => g.Key, g => g.Max(oc => oc.Efectivo));

            // Conversions => ToList

            List<Modulo> query18 = (from o in db.Modulos where o.ModuloId > 5 orderby o.Titulo select o).ToList();

            // Conversions => ToLookup

            ILookup<int, string> query19 = db.Modulos.ToLookup(c => c.ModuloId, c => c.Titulo); 

        }
    }

    static void Main(string[] args)
    {
        GenerarDatos();
        BorrarMatriculaciones();
        RealizarQuery();
    }

}