using CBR;

var outlook = new Email("smtp.office365.com", "jainepereira_projetocbr_sp@fia.com.br", ENV.PASSWORD);

outlook.CBR(
emailsTo: new List<string>
{
    "jainecarvalho15@outlook.com"
},
subject: "Testecbr1",
body: "Segue Anexo",
attachment: new List<string>
{
    @"C:\Users\jaine\folder1"
});
