﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NLIP.iShare.AuthorizationRegistry.Data;

namespace NLIP.iShare.AuthorizationRegistry.Data.Migrations
{
    [DbContext(typeof(AuthorizationRegistryDbContext))]
    [Migration("20180628084849_AddUserAndDelegationHistoryAndUpdateModels")]
    partial class AddUserAndDelegationHistoryAndUpdateModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NLIP.iShare.AuthorizationRegistry.Data.Models.Delegation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessSubject")
                        .IsRequired();

                    b.Property<string>("AuthorizationRegistryId")
                        .IsRequired();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("Inactive");

                    b.Property<string>("Policy")
                        .IsRequired();

                    b.Property<string>("PolicyIssuer")
                        .IsRequired();

                    b.Property<Guid>("UpdatedById");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Delegations");
                });

            modelBuilder.Entity("NLIP.iShare.AuthorizationRegistry.Data.Models.DelegationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("DelegationId");

                    b.Property<string>("Policy");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DelegationId");

                    b.ToTable("DelegationsHistories");
                });

            modelBuilder.Entity("NLIP.iShare.AuthorizationRegistry.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AspNetUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("Inactive");

                    b.Property<string>("Name");

                    b.Property<string>("PartyId");

                    b.Property<string>("PartyName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NLIP.iShare.AuthorizationRegistry.Data.Models.Delegation", b =>
                {
                    b.HasOne("NLIP.iShare.AuthorizationRegistry.Data.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("NLIP.iShare.AuthorizationRegistry.Data.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("NLIP.iShare.AuthorizationRegistry.Data.Models.DelegationHistory", b =>
                {
                    b.HasOne("NLIP.iShare.AuthorizationRegistry.Data.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NLIP.iShare.AuthorizationRegistry.Data.Models.Delegation", "Delegation")
                        .WithMany("DelegationHistory")
                        .HasForeignKey("DelegationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
