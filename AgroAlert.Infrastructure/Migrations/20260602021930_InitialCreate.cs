using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroAlert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AGRICULTORES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SENHA_HASH = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ATIVO = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGRICULTORES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HISTORICO_ACESSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ACAO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    ENDERECO_IP = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    SUCESSO = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    AgricultorId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORICO_ACESSO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HISTORICO_ACESSO_AGRICULTORES_AgricultorId",
                        column: x => x.AgricultorId,
                        principalTable: "AGRICULTORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROPRIEDADES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    LOCALIZACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    AREA_HECTARES = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    LATITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    LONGITUDE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TIPO_CULTURA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    AgricultorId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROPRIEDADES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PROPRIEDADES_AGRICULTORES_AgricultorId",
                        column: x => x.AgricultorId,
                        principalTable: "AGRICULTORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ALERTAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITULO = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    NIVEL_RISCO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_ALERTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LIDO = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_LEITURA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PropriedadeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ALERTAS_PROPRIEDADES_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "PROPRIEDADES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DADOS_CLIMATICOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TEMPERATURA = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    UMIDADE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PRECIPITACAO = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    VELOCIDADE_VENTO = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    FONTE_DADOS = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PropriedadeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DADOS_CLIMATICOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DADOS_CLIMATICOS_PROPRIEDADES_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "PROPRIEDADES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "REGRAS_ALERTA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    TipoAlerta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARAMETRO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    OPERADOR = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    VALOR_LIMITE = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    NivelRisco = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ATIVA = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    PropriedadeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REGRAS_ALERTA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REGRAS_ALERTA_PROPRIEDADES_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "PROPRIEDADES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AGRICULTORES_EMAIL",
                table: "AGRICULTORES",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ALERTAS_PropriedadeId",
                table: "ALERTAS",
                column: "PropriedadeId");

            migrationBuilder.CreateIndex(
                name: "IX_DADOS_CLIMATICOS_PropriedadeId",
                table: "DADOS_CLIMATICOS",
                column: "PropriedadeId");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORICO_ACESSO_AgricultorId",
                table: "HISTORICO_ACESSO",
                column: "AgricultorId");

            migrationBuilder.CreateIndex(
                name: "IX_PROPRIEDADES_AgricultorId",
                table: "PROPRIEDADES",
                column: "AgricultorId");

            migrationBuilder.CreateIndex(
                name: "IX_REGRAS_ALERTA_PropriedadeId",
                table: "REGRAS_ALERTA",
                column: "PropriedadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERTAS");

            migrationBuilder.DropTable(
                name: "DADOS_CLIMATICOS");

            migrationBuilder.DropTable(
                name: "HISTORICO_ACESSO");

            migrationBuilder.DropTable(
                name: "REGRAS_ALERTA");

            migrationBuilder.DropTable(
                name: "PROPRIEDADES");

            migrationBuilder.DropTable(
                name: "AGRICULTORES");
        }
    }
}
