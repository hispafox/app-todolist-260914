using AppTodoList.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<PlantillaTarea> Plantillas => Set<PlantillaTarea>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Persona> Personas => Set<Persona>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(todo => todo.Id);
            entity.Property(todo => todo.Title).IsRequired().HasMaxLength(200);
            entity.Property(todo => todo.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(todo => todo.IsCompleted).HasDefaultValue(false);
            entity.Property(todo => todo.EsRepetitiva).HasDefaultValue(false);

            entity.HasOne(todo => todo.Plantilla)
                .WithMany()
                .HasForeignKey(todo => todo.PlantillaId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(todo => todo.Categoria)
                .WithMany()
                .HasForeignKey(todo => todo.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(todo => todo.Persona)
                .WithMany()
                .HasForeignKey(todo => todo.PersonaId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PlantillaTarea>(entity =>
        {
            entity.HasKey(plantilla => plantilla.Id);
            entity.Property(plantilla => plantilla.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(plantilla => plantilla.EsRepetitiva).HasDefaultValue(false);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(categoria => categoria.Id);
            entity.Property(categoria => categoria.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(categoria => categoria.Color).HasMaxLength(30);
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(persona => persona.Id);
            entity.Property(persona => persona.Nombre).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Hogar", Color = "#4CAF50" },
            new Categoria { Id = 2, Nombre = "Trabajo", Color = "#2196F3" });

        modelBuilder.Entity<Persona>().HasData(
            new Persona { Id = 1, Nombre = "Ana López" },
            new Persona { Id = 2, Nombre = "Luis García" });

        modelBuilder.Entity<PlantillaTarea>().HasData(
            new PlantillaTarea { Id = 1, Titulo = "Revisar correo", EsRepetitiva = true, Recurrencia = TipoRecurrencia.Diaria },
            new PlantillaTarea { Id = 2, Titulo = "Preparar comida", EsRepetitiva = true, Recurrencia = TipoRecurrencia.Semanal });

        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem
            {
                Id = 1,
                Title = "Comprar pan",
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 9, 15, 8, 30, 0, DateTimeKind.Utc),
                EsRepetitiva = true,
                Recurrencia = TipoRecurrencia.Diaria,
                ProximaFecha = new DateTime(2026, 9, 16, 8, 30, 0, DateTimeKind.Utc),
                PlantillaId = 1,
                CategoriaId = 1,
                PersonaId = 1
            },
            new TodoItem
            {
                Id = 2,
                Title = "Enviar informe",
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 9, 15, 9, 0, 0, DateTimeKind.Utc),
                EsRepetitiva = false,
                Recurrencia = null,
                ProximaFecha = null,
                PlantillaId = null,
                CategoriaId = 2,
                PersonaId = 2
            });

        base.OnModelCreating(modelBuilder);
    }
}
