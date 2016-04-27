using System;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
    public class BasicPackageCreationExample : SdkSample
    {
        public readonly string Document1Name = "First Document";
        public readonly string Document2Name = "Second Document";

        public static void Main(string[] args)
        {
            new BasicPackageCreationExample().Run();
        }

        override public void Execute()
        {
            var superDuperPackage =
                PackageBuilder.NewPackageNamed(PackageName)
                .DescribedAs("This is a package created using the e-SignLive SDK")
                .ExpiresOn(DateTime.Now.AddMonths(100))
                .WithEmailMessage("This message should be delivered to all signers")
                .WithSigner(SignerBuilder.NewSignerWithEmail(email1)
                            .WithCustomId("Client1")
                            .WithFirstName("John")
                            .WithLastName("Smith")
                            .WithTitle("Managing Director")
                            .WithCompany("Acme Inc.")
                           )
                .WithSigner(SignerBuilder.NewSignerWithEmail(email2)
                            .WithFirstName("Patty")
                            .WithLastName("Galant")
                           )
                    .WithDocument(DocumentBuilder.NewDocumentNamed(Document1Name)
                              .FromStream(fileStream1, DocumentType.PDF)
                              .WithSignature(SignatureBuilder.SignatureFor(email1)
                                             .OnPage(0)
                                             .WithField(FieldBuilder.CheckBox()
                                                     .OnPage(0)
                                                     .AtPosition(400, 200)
                                                     .WithValue(FieldBuilder.CHECKBOX_CHECKED)
                                                       )
                                             .AtPosition(100, 100)
                                            )
                             )
                    .WithDocument(DocumentBuilder.NewDocumentNamed(Document2Name)
                              .FromStream(fileStream2, DocumentType.PDF)
                              .WithSignature(SignatureBuilder.SignatureFor(email2)
                                             .OnPage(0)
                                             .AtPosition(100, 200)
                                             .WithField(FieldBuilder.RadioButton("group")
                                                     .WithName("firstField")
                                                     .WithValue(false)
                                                     .WithSize(20, 20)
                                                     .OnPage(0)
                                                     .AtPosition(400, 200))
                                             .WithField(FieldBuilder.RadioButton("group")
                                                     .WithName("secondField")
                                                     .WithValue(true)
                                                     .WithSize(20, 20)
                                                     .OnPage(0)
                                                     .AtPosition(400, 250))
                                             .WithField(FieldBuilder.RadioButton("group")
                                                     .WithName("thirdField")
                                                     .WithValue(false)
                                                     .WithSize(20, 20)
                                                     .OnPage(0)
                                                     .AtPosition(400, 300))
                                            )
                             )
                .Build();

            packageId = eslClient.CreatePackageOneStep(superDuperPackage);
            eslClient.SendPackage(packageId);
            retrievedPackage = eslClient.GetPackage(packageId);
        }
    }
}
