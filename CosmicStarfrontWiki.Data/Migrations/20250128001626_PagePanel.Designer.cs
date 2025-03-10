﻿// <auto-generated />
using CosmicStarfrontWiki.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CosmicStarfrontWiki.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250128001626_PagePanel")]
    partial class PagePanel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageStyle")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SectionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Subheader")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Gallery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.PrimitiveCollection<string>("Captions")
                        .HasColumnType("TEXT");

                    b.PrimitiveCollection<string>("ImageStyles")
                        .HasColumnType("TEXT");

                    b.PrimitiveCollection<string>("Images")
                        .HasColumnType("TEXT");

                    b.Property<int>("WikiPageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WikiPageId")
                        .IsUnique();

                    b.ToTable("Galleries");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WikiPageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WikiPageId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.WikiPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageStyle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WikiPages");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Content", b =>
                {
                    b.HasOne("CosmicStarfrontWiki.Model.Section", "Section")
                        .WithMany("Contents")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Gallery", b =>
                {
                    b.HasOne("CosmicStarfrontWiki.Model.WikiPage", "WikiPage")
                        .WithOne("Gallery")
                        .HasForeignKey("CosmicStarfrontWiki.Model.Gallery", "WikiPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WikiPage");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Section", b =>
                {
                    b.HasOne("CosmicStarfrontWiki.Model.WikiPage", "WikiPage")
                        .WithMany("Sections")
                        .HasForeignKey("WikiPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WikiPage");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.Section", b =>
                {
                    b.Navigation("Contents");
                });

            modelBuilder.Entity("CosmicStarfrontWiki.Model.WikiPage", b =>
                {
                    b.Navigation("Gallery");

                    b.Navigation("Sections");
                });
#pragma warning restore 612, 618
        }
    }
}
