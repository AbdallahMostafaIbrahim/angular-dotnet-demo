// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApi.Models;

#nullable disable

namespace server.Migrations
{
    [DbContext(typeof(TodoDBContext))]
    [Migration("20220818101415_carts")]
    partial class carts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TodoApi.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("totalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Carts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            totalPrice = 200m,
                            userId = 1
                        },
                        new
                        {
                            Id = 2,
                            totalPrice = 200m,
                            userId = 2
                        });
                });

            modelBuilder.Entity("TodoApi.Models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("cartId")
                        .HasColumnType("int");

                    b.Property<int?>("productId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("cartId");

                    b.HasIndex("productId");

                    b.ToTable("CartItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            cartId = 1,
                            productId = 1
                        },
                        new
                        {
                            Id = 2,
                            cartId = 1,
                            productId = 2
                        },
                        new
                        {
                            Id = 3,
                            cartId = 2,
                            productId = 2
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("imageId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("imageId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            description = "nice",
                            imageId = 1,
                            name = "Electronics"
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Class");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            name = "Big"
                        },
                        new
                        {
                            Id = 2,
                            name = "Small"
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("imageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            imageUrl = "yeet.png",
                            name = "Image 1"
                        },
                        new
                        {
                            Id = 2,
                            imageUrl = "ok.png",
                            name = "Image 2"
                        },
                        new
                        {
                            Id = 3,
                            imageUrl = "adsf.png",
                            name = "Image 3"
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("categoryId")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("imageId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("categoryId");

                    b.HasIndex("imageId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            categoryId = 1,
                            description = "niasdfce",
                            imageId = 1,
                            name = "Cool",
                            price = 100m
                        },
                        new
                        {
                            Id = 2,
                            categoryId = 1,
                            description = "dsd",
                            imageId = 2,
                            name = "Very",
                            price = 123m
                        },
                        new
                        {
                            Id = 3,
                            categoryId = 1,
                            description = "nisadfasdfce",
                            imageId = 1,
                            name = "Dope",
                            price = 10120m
                        },
                        new
                        {
                            Id = 4,
                            categoryId = 1,
                            description = "asdf",
                            imageId = 3,
                            name = "Prod",
                            price = 10120m
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("classId")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("classId");

                    b.ToTable("Student");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            classId = 1,
                            email = "abdo@gmail.com",
                            username = "Abdallah"
                        },
                        new
                        {
                            Id = 2,
                            classId = 1,
                            email = "salma@gmail.com",
                            username = "Salma"
                        },
                        new
                        {
                            Id = 3,
                            classId = 2,
                            email = "hamza@gmail.com",
                            username = "Hamza"
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("isComplete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("TodoApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            email = "admin@gmail.com",
                            password = "$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$ZmFtaGxvd2xlZGdl",
                            username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            email = "user@gmail.com",
                            password = "$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$ZmFtaGxvd2xlZGdl",
                            username = "user"
                        });
                });

            modelBuilder.Entity("TodoApi.Models.Cart", b =>
                {
                    b.HasOne("TodoApi.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoApi.Models.CartItem", b =>
                {
                    b.HasOne("TodoApi.Models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("cartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoApi.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("productId");

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("TodoApi.Models.Category", b =>
                {
                    b.HasOne("TodoApi.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("imageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("TodoApi.Models.Product", b =>
                {
                    b.HasOne("TodoApi.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoApi.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("imageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("TodoApi.Models.Student", b =>
                {
                    b.HasOne("TodoApi.Models.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("classId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("TodoApi.Models.Todo", b =>
                {
                    b.HasOne("TodoApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoApi.Models.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("TodoApi.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("TodoApi.Models.Class", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("TodoApi.Models.User", b =>
                {
                    b.Navigation("Carts");
                });
#pragma warning restore 612, 618
        }
    }
}
