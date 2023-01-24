using ClassLibrary_MsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;

namespace ClassLibrary_MsTestTest
{
    [TestClass]
    public class FileProcessTest
    {
        private const string BAD_FILE_NAME = @"C:\Regedit.exe";
        private const string FILE_NAME = @"FileToDeploy.txt";

        private const string _CONN = "Data Source=127.0.0.1,1433; Initial Catalog=TesteUnitario; User ID=sa; Password=@Sql2022; Encrypt=False;";

        private string _GoodFileName;

        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]",
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData));
            }
        }

        //---------------------------------------------------------------------------------------------------
        // O TestContext é uma classe fornecida pelo Microsoft Test Framework (MsTest)
        //   que fornece informações sobre o contexto de teste atual,
        //   como o nome do teste sendo executado, o caminho do arquivo de teste, informações de depuração
        //   e dados de teste adicionais. Ele também fornece métodos para escrever informações
        //   de log durante a execução dos testes.
        //---------------------------------------------------------------------------------------------------
        public TestContext TestContext { get; set; }

        #region Test initialize and Cleanup

        //---------------------------------------------------------------------------------------------------
        //O método TestInitialize é um método de inicialização de teste no MSTest(Microsoft Test Framework)
        //que é executado antes de cada teste.Ele é usado para configurar o ambiente de teste,
        //como inicializar objetos ou configurar valores de propriedade.
        //---------------------------------------------------------------------------------------------------
        [TestInitialize]
        public void TestInitialize()
        {
            if (TestContext.TestName.StartsWith("FileNameDoesExists"))
            {
                SetGoodFileName();
                TestContext.WriteLine($"Creating File: {_GoodFileName}");

                File.AppendAllText(_GoodFileName, "Some Text");
                TestContext.WriteLine($"Writing some text in: {_GoodFileName}");
            }
        }

        //---------------------------------------------------------------------------------------------------
        //O método TestCleanup é um método de limpeza de teste no MSTest(Microsoft Test Framework)
        //que é executado após cada teste.Ele é usado para limpar o ambiente de teste, como desalocar
        //objetos ou limpar recursos alocados durante o teste. Ele é marcado com o atributo [TestCleanup]
        //e é chamado automaticamente pelo framework após cada teste.
        //---------------------------------------------------------------------------------------------------
        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.TestName.StartsWith("FileNameDoesExists"))
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine($"Deleting file: {_GoodFileName}");
                    File.Delete(_GoodFileName);
                }
            }
        }

        #endregion Test initialize and Cleanup

        //--------------------------------------------------------------------------
        //Maneiras de checar se um arquivo existe
        //--------------------------------------------------------------------------

        [TestMethod]
        [Description("Check file using database")]
        [Owner("Hal")]
        [DataSource("System.data.SqlClient", _CONN,
                    "FileProcessTest",
                    DataAccessMethod.Sequential)]
        public void FileTestFromDataBase()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();

            string fileName;
            bool expectedValue;
            bool causesException;
            bool fromCall;

            fileName = TestContext.DataRow["FileName"].ToString();
            expectedValue = Convert.ToBoolean((TestContext.DataRow["ExpectedValue"].ToString()));
            causesException = Convert.ToBoolean((TestContext.DataRow["CausesException"].ToString()));

            try
            {
                fromCall = fp.FileExists(fileName);

                Assert.AreEqual(expectedValue,
                                fromCall,
                                $"File{fileName} has failed. Method: FileTestFromDataBase()");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(causesException);
            }

            //ACT ------------------------------------------------------------
            fromCall = fp.FileExists(@"C:\Windows\Regedit.exe");

            //ASSERT-----------------------------------------------------------
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Description("Check if file does exists")]
        [Owner("Hal")]
        [TestCategory("No Exception")]
        [Priority(0)]
        public void FileNameDoesExists()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------
            fromCall = fp.FileExists(@"C:\Windows\Regedit.exe");

            //ASSERT-----------------------------------------------------------
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Description("Check if file does exists")]
        [Owner("Hal")]
        [TestCategory("No Exception")]
        [Priority(0)]
        public void FileNameDoesExists_UsingAppConfig_And_TestInitialize()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------

            //---> Context Initialize

            TestContext.WriteLine($"Testing File: {_GoodFileName}");
            fromCall = fp.FileExists(_GoodFileName);

            //---> Context Cleanup

            //ASSERT-----------------------------------------------------------
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Description("Check if file does exists")]
        [TestCategory("No Exception")]
        [Owner("Hal")]
        [Priority(0)]
        public void FileNameDoesExists_UsingAppConfig()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------
            SetGoodFileName();
            TestContext.WriteLine($"Creating File: {_GoodFileName}");

            File.AppendAllText(_GoodFileName, "Some Text");
            TestContext.WriteLine($"Writing some text in: {_GoodFileName}");

            fromCall = fp.FileExists(_GoodFileName);
            File.Delete(_GoodFileName);
            TestContext.WriteLine($"Deleting file: {_GoodFileName}");

            //ASSERT-----------------------------------------------------------
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Priority(1)]
        [Description("Check if file does not exists")]
        [TestCategory("No Exception")]
        [Owner("Guilherme")]
        public void FileNameDoesNotExists()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------
            fromCall = fp.FileExists(BAD_FILE_NAME);

            //ASSERT-----------------------------------------------------------
            Assert.IsFalse(fromCall);
        }

        //--------------------------------------------------------------------------------------------
        //DeploymentItem é um atributo no MSTest que é usado para especificar um arquivo ou diretório
        //que deve ser copiado para o diretório de teste antes de uma classe de teste ser executada.
        //Isso é útil quando você precisa de arquivos adicionais, como arquivos de configuração
        //ou dados de teste, para ser disponíveis para sua classe de teste.
        //--------------------------------------------------------------------------------------------
        [TestMethod]
        [Owner("Hal")]
        [DeploymentItem(FILE_NAME)]
        [Description("Check if file does not exists")]
        [TestCategory("No Exception")]
        public void FileNameDoesExists_UsingDeploymentItems()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            string fileName;
            bool fromCall;

            //ACT ------------------------------------------------------------

            fileName = $@"{TestContext.DeploymentDirectory}\{FILE_NAME}";

            TestContext.WriteLine($"Testing File: {fileName}");
            fromCall = fp.FileExists(fileName);

            //ASSERT-----------------------------------------------------------
            Assert.IsTrue(fromCall);
        }

        //--------------------------------------------------------------------------
        //Usando Asserts para emitir mensagens de teste
        //--------------------------------------------------------------------------
        [TestMethod]
        [Description("Check if file does not exists")]
        [TestCategory("No Exception")]
        public void FileNameDoesExists_SimpleMessage()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------
            TestContext.WriteLine($"Testing files: {_GoodFileName}");
            fromCall = fp.FileExists(_GoodFileName);

            //ASSERT-----------------------------------------------------------
            Assert.IsFalse(fromCall, "File DOES NOT exists.");
        }

        [TestMethod]
        [Description("Check if file does not exists")]
        [TestCategory("No Exception")]
        public void FileNameDoesExists_MessageFormatting()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------
            TestContext.WriteLine($"Testing files: {_GoodFileName}");
            fromCall = fp.FileExists(_GoodFileName);

            //ASSERT-----------------------------------------------------------
            Assert.IsFalse(fromCall, "File {0} DOES NOT exists.", _GoodFileName);
        }

        //--------------------------------------------------------------------------
        //Maneiras trabalhar com exceptions
        //--------------------------------------------------------------------------

        [TestMethod]
        [Priority(1)]
        [Description("Check if file id null or empty")]
        [TestCategory("Exception")]
        [Owner("Ana")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            //ACT ------------------------------------------------------------
            fromCall = fp.FileExists("");
        }

        [TestMethod]
        [Description("Check if file id null or empty")]
        [TestCategory("Exception")]
        [Owner("Ana")]
        [Ignore] //-------> Ignora o teste
        public void FileNameNullOrEmpty_ThrowsArgumentNullException_UsingTryCatch()
        {
            //ARRANGE --------------------------------------------------------
            FileProcess fp = new FileProcess();
            bool fromCall;

            try
            {
                //ACT ------------------------------------------------------------
                fromCall = fp.FileExists("");
            }
            catch (ArgumentException)
            {
                return;
            }

            Assert.Fail("Fail expected");
        }

        [TestMethod]
        [Description("Test timeout of 2 seconds")]
        [Timeout(2000)] //--> Condiçao timeout de sucesso
        [TestCategory("Exception")]
        [Owner("Ana")]
        public void SimulateTimeout()
        {
            System.Threading.Thread.Sleep(3000);
        }
    }
}