using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;
using GST.Library.Shared.Mock;

namespace GST.Library.StoredProcedureHelper.Tests
{
    public class StoredProcedureTest
    {
        [Fact]
        public void MustCount()
        {
            var dbCommandMock = new Mock<IDbCommand>();

            var dbParameterCollectionMock = new Mock<IDataParameterCollection>();
            var dbConnectionMock = new Mock<IDbConnection>();


            dbCommandMock.Setup(s => s.ExecuteScalar()).Returns((long)42);
            dbCommandMock.Setup(s => s.Connection).Returns(dbConnectionMock.Object);
            dbCommandMock.SetupGet(s => s.Parameters).Returns(dbParameterCollectionMock.Object);

            StoredProcedure<CheckType> sp = new StoredProcedure<CheckType>(dbCommandMock.Object, "CeciEstLeNomDUneFonctionStockee");

            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "isAInt", "123" },
                { "isAString", "ot" },
            };

            sp.parameters = parameters;

            Assert.Equal(42, sp.Count());

            dbCommandMock.VerifySet(v => v.CommandText = "SELECT COUNT(*) FROM CeciEstLeNomDUneFonctionStockee(@isAInt,@isAString)", Times.Once);
            dbCommandMock.Verify(v => v.ExecuteScalar(), Times.Once);
            dbParameterCollectionMock.Verify(v => v.Clear(), Times.Once);
        }

        [Fact]
        public void MustSelect()
        {
            var dbCommandMock = new Mock<IDbCommand>();

            var dbParameterCollectionMock = new Mock<IDataParameterCollection>();
            var dbConnectionMock = new Mock<IDbConnection>();
            var dataReaderMock = new Mock<IDataReader>();
            dataReaderMock.Setup(s => s.FieldCount).Returns(8);

            dataReaderMock.Setup(s => s.GetName(0)).Returns("isAString");
            dataReaderMock.Setup(s => s.GetValue(0)).Returns("isAString");

            dataReaderMock.Setup(s => s.GetName(1)).Returns("isADateTime");
            dataReaderMock.Setup(s => s.GetValue(1)).Returns(new DateTime());

            dataReaderMock.Setup(s => s.GetName(2)).Returns("isAInt");
            dataReaderMock.Setup(s => s.GetValue(2)).Returns((int)42);

            dataReaderMock.Setup(s => s.GetName(3)).Returns("isAInt16");
            dataReaderMock.Setup(s => s.GetValue(3)).Returns((Int16)420);

            dataReaderMock.Setup(s => s.GetName(4)).Returns("isAInt32");
            dataReaderMock.Setup(s => s.GetValue(4)).Returns((Int32)4200);

            dataReaderMock.Setup(s => s.GetName(5)).Returns("isAInt64");
            dataReaderMock.Setup(s => s.GetValue(5)).Returns((Int64)42000);

            dataReaderMock.Setup(s => s.GetName(6)).Returns("isADouble");
            dataReaderMock.Setup(s => s.GetValue(6)).Returns((double)420000);

            dataReaderMock.Setup(s => s.GetName(7)).Returns("isAFloat");
            dataReaderMock.Setup(s => s.GetValue(7)).Returns((float)4200000);

            dataReaderMock.SetupSequence(m => m.Read())
            .Returns(true)
            .Returns(true)
            .Returns(false);


            dbCommandMock.Setup(s => s.ExecuteScalar()).Returns((long)42);
            dbCommandMock.Setup(s => s.Connection).Returns(dbConnectionMock.Object);
            dbCommandMock.SetupGet(s => s.Parameters).Returns(dbParameterCollectionMock.Object);
            dbCommandMock.Setup(s => s.ExecuteReader()).Returns(dataReaderMock.Object);

            StoredProcedure<CheckType> sp = new StoredProcedure<CheckType>(dbCommandMock.Object, "CeciEstLeNomDUneFonctionStockee");

            sp.limit = 123;
            sp.offset = 42;
            sp.order = new string[] { "isAString" };
            sp.orderDir = "desc";
            sp.search = "555555";

            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "isAInt", "123" },
                { "isAString", "ot" },
            };

            sp.parameters = parameters;

            List<CheckType> checkTypes = sp.ToList();

            Assert.Equal("isAString", checkTypes[0].isAString);

            dbCommandMock.VerifySet(v => v.CommandText = "SELECT * FROM CeciEstLeNomDUneFonctionStockee(@isAInt,@isAString) WHERE  lower(isAString) LIKE @propSearchisastring  OR  isAInt = @propSearchisaint OR  isAInt16 = @propSearchisaint16 OR  isAInt32 = @propSearchisaint32 OR  isAInt64 = @propSearchisaint64 OR  isADouble = @propSearchisadouble OR  isAFloat = @propSearchisafloat ORDER BY desc LIMIT 123 OFFSET 42", Times.Once);
            dbCommandMock.Verify(v => v.ExecuteScalar(), Times.Never);
            dbParameterCollectionMock.Verify(v => v.Clear(), Times.Once);
        }
    }
}
