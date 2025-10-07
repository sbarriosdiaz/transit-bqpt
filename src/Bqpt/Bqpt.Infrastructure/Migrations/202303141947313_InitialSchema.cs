namespace Bqpt.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachment",
                c => new
                {
                    AttachmentId = c.String(nullable: false, maxLength: 48, unicode: false),
                    BlobUrl = c.String(nullable: false, maxLength: 2048, unicode: false),
                    FileName = c.String(nullable: false, maxLength: 128, unicode: false),
                    FileExtension = c.String(nullable: false, maxLength: 10, unicode: false),
                    FileSizeKb = c.Double(nullable: false),
                    AttachmentFriendlyName = c.String(nullable: false, maxLength: 256, unicode: false),
                    Status = c.String(nullable: false, maxLength: 18, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.AttachmentId);

            CreateTable(
                "dbo.BidQuoteNote",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Note = c.String(maxLength: 2048, unicode: false),
                    BidQuoteId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BidQuote", t => t.BidQuoteId)
                .Index(t => t.BidQuoteId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.BidQuote",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AssetWorksBidQuoteId = c.String(nullable: false, maxLength: 48, unicode: false),
                    AssetWorksDateInserted = c.DateTime(),
                    AssetWorksDateApproval = c.DateTime(),
                    AssetWorksDateRequired = c.DateTime(),
                    AssetWorksDateRequested = c.DateTime(),
                    AssetWorksTotalCost = c.Decimal(precision: 18, scale: 2),
                    BidScheduledStartTime = c.DateTime(),
                    BidScheduledEndTime = c.DateTime(),
                    BidStartTime = c.DateTime(),
                    BidEndTime = c.DateTime(),
                    BidImportedTime = c.DateTime(),
                    BidClosedTime = c.DateTime(),
                    Status = c.String(nullable: false, maxLength: 24, unicode: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.BidQuotePart",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AssetWorksPartNumber = c.String(nullable: false, maxLength: 48, unicode: false),
                    AssetWorksPartCommodityCode = c.String(nullable: false, maxLength: 48, unicode: false),
                    AssetWorksPartManufacturerPartNumber = c.String(maxLength: 48, unicode: false),
                    AssetWorksPartSuffix = c.String(maxLength: 48, unicode: false),
                    AssetWorksPartDescription = c.String(maxLength: 1024, unicode: false),
                    AssetWorksUnitPrice = c.String(maxLength: 12, unicode: false),
                    AssetWorksPartCategory = c.String(maxLength: 48, unicode: false),
                    AssetWorksPartLineNumber = c.String(maxLength: 12, unicode: false),
                    AssetWorksMinPartLineNumber = c.String(unicode: false),
                    AssetWorksPartLocationCode = c.String(maxLength: 48, unicode: false),
                    AssetWorksPartDateInserted = c.DateTime(),
                    AssetWorksPartDateRequired = c.DateTime(),
                    AssetWorksPartQuantityRequested = c.Int(),
                    AssetWorksPartQuantityOnHand = c.Int(),
                    AssetWorksPartQuantityOnOrder = c.Int(),
                    AssetWorksPartQuantityCommited = c.Int(),
                    RequestedQuantity = c.Int(),
                    PurchaseOrderQuantity = c.Int(),
                    BidQuoteId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BidQuote", t => t.BidQuoteId)
                .Index(t => t.BidQuoteId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.VendorPartBid",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    QuantityAvailable = c.Int(),
                    ItemsPerUnit = c.Int(),
                    UnitPrice = c.Decimal(precision: 18, scale: 2),
                    EstimateDeliveryDate = c.DateTime(),
                    Comment = c.String(maxLength: 1024, unicode: false),
                    IsSelected = c.Boolean(),
                    VendorId = c.Int(nullable: false),
                    BidQuotePartId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vendor", t => t.VendorId)
                .ForeignKey("dbo.BidQuotePart", t => t.BidQuotePartId)
                .Index(t => t.VendorId)
                .Index(t => t.BidQuotePartId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.Vendor",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AssetWorksVendorNumber = c.String(nullable: false, maxLength: 48, unicode: false),
                    AssetWorksVendorAccountingSystemNumber = c.String(maxLength: 48, unicode: false),
                    AssetWorksVendorName = c.String(maxLength: 256, unicode: false),
                    AssetWorksVendorContactName = c.String(maxLength: 128, unicode: false),
                    AssetWorksVendorAddress1 = c.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress2 = c.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress3 = c.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress4 = c.String(maxLength: 256, unicode: false),
                    AssetWorksVendorEmail = c.String(nullable: false, maxLength: 256, unicode: false),
                    AssetWorksVendorPhone = c.String(maxLength: 24, unicode: false),
                    AssetWorksVendorFax = c.String(maxLength: 24, unicode: false),
                    CommodityCodes = c.String(maxLength: 512, unicode: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.Contact",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AssetWorksContactName = c.String(nullable: false, maxLength: 128, unicode: false),
                    AssetWorksContactPhone = c.String(maxLength: 96, unicode: false),
                    AssetWorksContactEmail = c.String(nullable: false, maxLength: 256, unicode: false),
                    AssetWorksContactAddress1 = c.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress2 = c.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress3 = c.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress4 = c.String(maxLength: 256, unicode: false),
                    VendorId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vendor", t => t.VendorId)
                .Index(t => t.VendorId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.VendorBidQuoteAttachment",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Comment = c.String(maxLength: 1024, unicode: false),
                    VendorId = c.Int(nullable: false),
                    BidQuoteId = c.Int(nullable: false),
                    AttachmentId = c.String(maxLength: 48, unicode: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attachment", t => t.AttachmentId)
                .ForeignKey("dbo.Vendor", t => t.VendorId)
                .ForeignKey("dbo.BidQuote", t => t.BidQuoteId)
                .Index(t => t.VendorId)
                .Index(t => t.BidQuoteId)
                .Index(t => t.AttachmentId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.VendorInPart",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NotificationSent = c.DateTime(nullable: false),
                    BidQuotePartId = c.Int(nullable: false),
                    VendorId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vendor", t => t.VendorId)
                .ForeignKey("dbo.BidQuotePart", t => t.BidQuotePartId)
                .Index(t => t.BidQuotePartId)
                .Index(t => t.VendorId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.VendorPartBidHistory",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    QuantityAvailable = c.Int(),
                    ItemsPerUnit = c.Int(),
                    UnitPrice = c.Decimal(precision: 18, scale: 2),
                    EstimateDeliveryDate = c.DateTime(),
                    Comment = c.String(maxLength: 1024, unicode: false),
                    IsSelected = c.Boolean(),
                    LoggedInEmailAddress = c.String(unicode: false),
                    VendorPartBidId = c.Int(nullable: false),
                    VendorPartBidNoteId = c.Int(),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VendorPartBid", t => t.VendorPartBidId, cascadeDelete: true)
                .Index(t => t.VendorPartBidId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.VendorPartBidSelectedHistory",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    VendorPartBidId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VendorPartBid", t => t.VendorPartBidId)
                .Index(t => t.VendorPartBidId)
                .Index(t => t.DomainKey, unique: true);

            CreateTable(
                "dbo.BidQuoteStatusHistory",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Status = c.String(nullable: false, maxLength: 24, unicode: false),
                    BidQuoteId = c.Int(nullable: false),
                    DomainKey = c.String(nullable: false, maxLength: 48, unicode: false),
                    DisplayOrder = c.Byte(),
                    IsDeleted = c.Boolean(),
                    DeletedBy = c.String(maxLength: 48, unicode: false),
                    DeletedOn = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                    CreatedBy = c.String(maxLength: 48, unicode: false),
                    CreatedOn = c.DateTime(),
                    UpdatedBy = c.String(maxLength: 48, unicode: false),
                    UpdatedOn = c.DateTime(),
                    DeactivatedBy = c.String(maxLength: 48, unicode: false),
                    DeactivatedOn = c.DateTime(),
                    Description = c.String(maxLength: 512, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BidQuote", t => t.BidQuoteId)
                .Index(t => t.BidQuoteId)
                .Index(t => t.DomainKey, unique: true);

            CreateStoredProcedure(
                "dbo.Attachment_Insert",
                p => new
                {
                    AttachmentId = p.String(maxLength: 48, unicode: false),
                    BlobUrl = p.String(maxLength: 2048, unicode: false),
                    FileName = p.String(maxLength: 128, unicode: false),
                    FileExtension = p.String(maxLength: 10, unicode: false),
                    FileSizeKb = p.Double(),
                    AttachmentFriendlyName = p.String(maxLength: 256, unicode: false),
                    Status = p.String(maxLength: 18, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[Attachment]([AttachmentId], [BlobUrl], [FileName], [FileExtension], [FileSizeKb], [AttachmentFriendlyName], [Status], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@AttachmentId, @BlobUrl, @FileName, @FileExtension, @FileSizeKb, @AttachmentFriendlyName, @Status, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)"
            );

            CreateStoredProcedure(
                "dbo.Attachment_Update",
                p => new
                {
                    AttachmentId = p.String(maxLength: 48, unicode: false),
                    BlobUrl = p.String(maxLength: 2048, unicode: false),
                    FileName = p.String(maxLength: 128, unicode: false),
                    FileExtension = p.String(maxLength: 10, unicode: false),
                    FileSizeKb = p.Double(),
                    AttachmentFriendlyName = p.String(maxLength: 256, unicode: false),
                    Status = p.String(maxLength: 18, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[Attachment]
                      SET [BlobUrl] = @BlobUrl, [FileName] = @FileName, [FileExtension] = @FileExtension, [FileSizeKb] = @FileSizeKb, [AttachmentFriendlyName] = @AttachmentFriendlyName, [Status] = @Status, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([AttachmentId] = @AttachmentId)"
            );

            CreateStoredProcedure(
                "dbo.Attachment_Delete",
                p => new
                {
                    AttachmentId = p.String(maxLength: 48, unicode: false),
                },
                body:
                    @"DELETE [dbo].[Attachment]
                      WHERE ([AttachmentId] = @AttachmentId)"
            );

            CreateStoredProcedure(
                "dbo.BidQuoteNote_Insert",
                p => new
                {
                    Note = p.String(maxLength: 2048, unicode: false),
                    BidQuoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[BidQuoteNote]([Note], [BidQuoteId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@Note, @BidQuoteId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BidQuoteNote]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[BidQuoteNote] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.BidQuoteNote_Update",
                p => new
                {
                    Id = p.Int(),
                    Note = p.String(maxLength: 2048, unicode: false),
                    BidQuoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[BidQuoteNote]
                      SET [Note] = @Note, [BidQuoteId] = @BidQuoteId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuoteNote_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[BidQuoteNote]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuote_Insert",
                p => new
                {
                    AssetWorksBidQuoteId = p.String(maxLength: 48, unicode: false),
                    AssetWorksDateInserted = p.DateTime(),
                    AssetWorksDateApproval = p.DateTime(),
                    AssetWorksDateRequired = p.DateTime(),
                    AssetWorksDateRequested = p.DateTime(),
                    AssetWorksTotalCost = p.Decimal(precision: 18, scale: 2),
                    BidScheduledStartTime = p.DateTime(),
                    BidScheduledEndTime = p.DateTime(),
                    BidStartTime = p.DateTime(),
                    BidEndTime = p.DateTime(),
                    BidImportedTime = p.DateTime(),
                    BidClosedTime = p.DateTime(),
                    Status = p.String(maxLength: 24, unicode: false),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[BidQuote]([AssetWorksBidQuoteId], [AssetWorksDateInserted], [AssetWorksDateApproval], [AssetWorksDateRequired], [AssetWorksDateRequested], [AssetWorksTotalCost], [BidScheduledStartTime], [BidScheduledEndTime], [BidStartTime], [BidEndTime], [BidImportedTime], [BidClosedTime], [Status], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@AssetWorksBidQuoteId, @AssetWorksDateInserted, @AssetWorksDateApproval, @AssetWorksDateRequired, @AssetWorksDateRequested, @AssetWorksTotalCost, @BidScheduledStartTime, @BidScheduledEndTime, @BidStartTime, @BidEndTime, @BidImportedTime, @BidClosedTime, @Status, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BidQuote]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[BidQuote] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.BidQuote_Update",
                p => new
                {
                    Id = p.Int(),
                    AssetWorksBidQuoteId = p.String(maxLength: 48, unicode: false),
                    AssetWorksDateInserted = p.DateTime(),
                    AssetWorksDateApproval = p.DateTime(),
                    AssetWorksDateRequired = p.DateTime(),
                    AssetWorksDateRequested = p.DateTime(),
                    AssetWorksTotalCost = p.Decimal(precision: 18, scale: 2),
                    BidScheduledStartTime = p.DateTime(),
                    BidScheduledEndTime = p.DateTime(),
                    BidStartTime = p.DateTime(),
                    BidEndTime = p.DateTime(),
                    BidImportedTime = p.DateTime(),
                    BidClosedTime = p.DateTime(),
                    Status = p.String(maxLength: 24, unicode: false),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[BidQuote]
                      SET [AssetWorksBidQuoteId] = @AssetWorksBidQuoteId, [AssetWorksDateInserted] = @AssetWorksDateInserted, [AssetWorksDateApproval] = @AssetWorksDateApproval, [AssetWorksDateRequired] = @AssetWorksDateRequired, [AssetWorksDateRequested] = @AssetWorksDateRequested, [AssetWorksTotalCost] = @AssetWorksTotalCost, [BidScheduledStartTime] = @BidScheduledStartTime, [BidScheduledEndTime] = @BidScheduledEndTime, [BidStartTime] = @BidStartTime, [BidEndTime] = @BidEndTime, [BidImportedTime] = @BidImportedTime, [BidClosedTime] = @BidClosedTime, [Status] = @Status, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuote_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[BidQuote]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuotePart_Insert",
                p => new
                {
                    AssetWorksPartNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartCommodityCode = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartManufacturerPartNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartSuffix = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartDescription = p.String(maxLength: 1024, unicode: false),
                    AssetWorksUnitPrice = p.String(maxLength: 12, unicode: false),
                    AssetWorksPartCategory = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartLineNumber = p.String(maxLength: 12, unicode: false),
                    AssetWorksMinPartLineNumber = p.String(unicode: false),
                    AssetWorksPartLocationCode = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartDateInserted = p.DateTime(),
                    AssetWorksPartDateRequired = p.DateTime(),
                    AssetWorksPartQuantityRequested = p.Int(),
                    AssetWorksPartQuantityOnHand = p.Int(),
                    AssetWorksPartQuantityOnOrder = p.Int(),
                    AssetWorksPartQuantityCommited = p.Int(),
                    RequestedQuantity = p.Int(),
                    PurchaseOrderQuantity = p.Int(),
                    BidQuoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[BidQuotePart]([AssetWorksPartNumber], [AssetWorksPartCommodityCode], [AssetWorksPartManufacturerPartNumber], [AssetWorksPartSuffix], [AssetWorksPartDescription], [AssetWorksUnitPrice], [AssetWorksPartCategory], [AssetWorksPartLineNumber], [AssetWorksMinPartLineNumber], [AssetWorksPartLocationCode], [AssetWorksPartDateInserted], [AssetWorksPartDateRequired], [AssetWorksPartQuantityRequested], [AssetWorksPartQuantityOnHand], [AssetWorksPartQuantityOnOrder], [AssetWorksPartQuantityCommited], [RequestedQuantity], [PurchaseOrderQuantity], [BidQuoteId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@AssetWorksPartNumber, @AssetWorksPartCommodityCode, @AssetWorksPartManufacturerPartNumber, @AssetWorksPartSuffix, @AssetWorksPartDescription, @AssetWorksUnitPrice, @AssetWorksPartCategory, @AssetWorksPartLineNumber, @AssetWorksMinPartLineNumber, @AssetWorksPartLocationCode, @AssetWorksPartDateInserted, @AssetWorksPartDateRequired, @AssetWorksPartQuantityRequested, @AssetWorksPartQuantityOnHand, @AssetWorksPartQuantityOnOrder, @AssetWorksPartQuantityCommited, @RequestedQuantity, @PurchaseOrderQuantity, @BidQuoteId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BidQuotePart]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[BidQuotePart] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.BidQuotePart_Update",
                p => new
                {
                    Id = p.Int(),
                    AssetWorksPartNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartCommodityCode = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartManufacturerPartNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartSuffix = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartDescription = p.String(maxLength: 1024, unicode: false),
                    AssetWorksUnitPrice = p.String(maxLength: 12, unicode: false),
                    AssetWorksPartCategory = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartLineNumber = p.String(maxLength: 12, unicode: false),
                    AssetWorksMinPartLineNumber = p.String(unicode: false),
                    AssetWorksPartLocationCode = p.String(maxLength: 48, unicode: false),
                    AssetWorksPartDateInserted = p.DateTime(),
                    AssetWorksPartDateRequired = p.DateTime(),
                    AssetWorksPartQuantityRequested = p.Int(),
                    AssetWorksPartQuantityOnHand = p.Int(),
                    AssetWorksPartQuantityOnOrder = p.Int(),
                    AssetWorksPartQuantityCommited = p.Int(),
                    RequestedQuantity = p.Int(),
                    PurchaseOrderQuantity = p.Int(),
                    BidQuoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[BidQuotePart]
                      SET [AssetWorksPartNumber] = @AssetWorksPartNumber, [AssetWorksPartCommodityCode] = @AssetWorksPartCommodityCode, [AssetWorksPartManufacturerPartNumber] = @AssetWorksPartManufacturerPartNumber, [AssetWorksPartSuffix] = @AssetWorksPartSuffix, [AssetWorksPartDescription] = @AssetWorksPartDescription, [AssetWorksUnitPrice] = @AssetWorksUnitPrice, [AssetWorksPartCategory] = @AssetWorksPartCategory, [AssetWorksPartLineNumber] = @AssetWorksPartLineNumber, [AssetWorksMinPartLineNumber] = @AssetWorksMinPartLineNumber, [AssetWorksPartLocationCode] = @AssetWorksPartLocationCode, [AssetWorksPartDateInserted] = @AssetWorksPartDateInserted, [AssetWorksPartDateRequired] = @AssetWorksPartDateRequired, [AssetWorksPartQuantityRequested] = @AssetWorksPartQuantityRequested, [AssetWorksPartQuantityOnHand] = @AssetWorksPartQuantityOnHand, [AssetWorksPartQuantityOnOrder] = @AssetWorksPartQuantityOnOrder, [AssetWorksPartQuantityCommited] = @AssetWorksPartQuantityCommited, [RequestedQuantity] = @RequestedQuantity, [PurchaseOrderQuantity] = @PurchaseOrderQuantity, [BidQuoteId] = @BidQuoteId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuotePart_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[BidQuotePart]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBid_Insert",
                p => new
                {
                    QuantityAvailable = p.Int(),
                    ItemsPerUnit = p.Int(),
                    UnitPrice = p.Decimal(precision: 18, scale: 2),
                    EstimateDeliveryDate = p.DateTime(),
                    Comment = p.String(maxLength: 1024, unicode: false),
                    IsSelected = p.Boolean(),
                    VendorId = p.Int(),
                    BidQuotePartId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[VendorPartBid]([QuantityAvailable], [ItemsPerUnit], [UnitPrice], [EstimateDeliveryDate], [Comment], [IsSelected], [VendorId], [BidQuotePartId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@QuantityAvailable, @ItemsPerUnit, @UnitPrice, @EstimateDeliveryDate, @Comment, @IsSelected, @VendorId, @BidQuotePartId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorPartBid]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[VendorPartBid] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBid_Update",
                p => new
                {
                    Id = p.Int(),
                    QuantityAvailable = p.Int(),
                    ItemsPerUnit = p.Int(),
                    UnitPrice = p.Decimal(precision: 18, scale: 2),
                    EstimateDeliveryDate = p.DateTime(),
                    Comment = p.String(maxLength: 1024, unicode: false),
                    IsSelected = p.Boolean(),
                    VendorId = p.Int(),
                    BidQuotePartId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[VendorPartBid]
                      SET [QuantityAvailable] = @QuantityAvailable, [ItemsPerUnit] = @ItemsPerUnit, [UnitPrice] = @UnitPrice, [EstimateDeliveryDate] = @EstimateDeliveryDate, [Comment] = @Comment, [IsSelected] = @IsSelected, [VendorId] = @VendorId, [BidQuotePartId] = @BidQuotePartId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBid_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[VendorPartBid]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.Vendor_Insert",
                p => new
                {
                    AssetWorksVendorNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksVendorAccountingSystemNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksVendorName = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorContactName = p.String(maxLength: 128, unicode: false),
                    AssetWorksVendorAddress1 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress2 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress3 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress4 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorEmail = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorPhone = p.String(maxLength: 24, unicode: false),
                    AssetWorksVendorFax = p.String(maxLength: 24, unicode: false),
                    CommodityCodes = p.String(maxLength: 512, unicode: false),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[Vendor]([AssetWorksVendorNumber], [AssetWorksVendorAccountingSystemNumber], [AssetWorksVendorName], [AssetWorksVendorContactName], [AssetWorksVendorAddress1], [AssetWorksVendorAddress2], [AssetWorksVendorAddress3], [AssetWorksVendorAddress4], [AssetWorksVendorEmail], [AssetWorksVendorPhone], [AssetWorksVendorFax], [CommodityCodes], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@AssetWorksVendorNumber, @AssetWorksVendorAccountingSystemNumber, @AssetWorksVendorName, @AssetWorksVendorContactName, @AssetWorksVendorAddress1, @AssetWorksVendorAddress2, @AssetWorksVendorAddress3, @AssetWorksVendorAddress4, @AssetWorksVendorEmail, @AssetWorksVendorPhone, @AssetWorksVendorFax, @CommodityCodes, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Vendor]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[Vendor] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.Vendor_Update",
                p => new
                {
                    Id = p.Int(),
                    AssetWorksVendorNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksVendorAccountingSystemNumber = p.String(maxLength: 48, unicode: false),
                    AssetWorksVendorName = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorContactName = p.String(maxLength: 128, unicode: false),
                    AssetWorksVendorAddress1 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress2 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress3 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorAddress4 = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorEmail = p.String(maxLength: 256, unicode: false),
                    AssetWorksVendorPhone = p.String(maxLength: 24, unicode: false),
                    AssetWorksVendorFax = p.String(maxLength: 24, unicode: false),
                    CommodityCodes = p.String(maxLength: 512, unicode: false),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[Vendor]
                      SET [AssetWorksVendorNumber] = @AssetWorksVendorNumber, [AssetWorksVendorAccountingSystemNumber] = @AssetWorksVendorAccountingSystemNumber, [AssetWorksVendorName] = @AssetWorksVendorName, [AssetWorksVendorContactName] = @AssetWorksVendorContactName, [AssetWorksVendorAddress1] = @AssetWorksVendorAddress1, [AssetWorksVendorAddress2] = @AssetWorksVendorAddress2, [AssetWorksVendorAddress3] = @AssetWorksVendorAddress3, [AssetWorksVendorAddress4] = @AssetWorksVendorAddress4, [AssetWorksVendorEmail] = @AssetWorksVendorEmail, [AssetWorksVendorPhone] = @AssetWorksVendorPhone, [AssetWorksVendorFax] = @AssetWorksVendorFax, [CommodityCodes] = @CommodityCodes, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.Vendor_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[Vendor]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                {
                    AssetWorksContactName = p.String(maxLength: 128, unicode: false),
                    AssetWorksContactPhone = p.String(maxLength: 96, unicode: false),
                    AssetWorksContactEmail = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress1 = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress2 = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress3 = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress4 = p.String(maxLength: 256, unicode: false),
                    VendorId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[Contact]([AssetWorksContactName], [AssetWorksContactPhone], [AssetWorksContactEmail], [AssetWorksContactAddress1], [AssetWorksContactAddress2], [AssetWorksContactAddress3], [AssetWorksContactAddress4], [VendorId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@AssetWorksContactName, @AssetWorksContactPhone, @AssetWorksContactEmail, @AssetWorksContactAddress1, @AssetWorksContactAddress2, @AssetWorksContactAddress3, @AssetWorksContactAddress4, @VendorId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Contact]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[Contact] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.Contact_Update",
                p => new
                {
                    Id = p.Int(),
                    AssetWorksContactName = p.String(maxLength: 128, unicode: false),
                    AssetWorksContactPhone = p.String(maxLength: 96, unicode: false),
                    AssetWorksContactEmail = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress1 = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress2 = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress3 = p.String(maxLength: 256, unicode: false),
                    AssetWorksContactAddress4 = p.String(maxLength: 256, unicode: false),
                    VendorId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[Contact]
                      SET [AssetWorksContactName] = @AssetWorksContactName, [AssetWorksContactPhone] = @AssetWorksContactPhone, [AssetWorksContactEmail] = @AssetWorksContactEmail, [AssetWorksContactAddress1] = @AssetWorksContactAddress1, [AssetWorksContactAddress2] = @AssetWorksContactAddress2, [AssetWorksContactAddress3] = @AssetWorksContactAddress3, [AssetWorksContactAddress4] = @AssetWorksContactAddress4, [VendorId] = @VendorId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.Contact_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[Contact]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorBidQuoteAttachment_Insert",
                p => new
                {
                    Comment = p.String(maxLength: 1024, unicode: false),
                    VendorId = p.Int(),
                    BidQuoteId = p.Int(),
                    AttachmentId = p.String(maxLength: 48, unicode: false),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[VendorBidQuoteAttachment]([Comment], [VendorId], [BidQuoteId], [AttachmentId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@Comment, @VendorId, @BidQuoteId, @AttachmentId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorBidQuoteAttachment]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[VendorBidQuoteAttachment] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.VendorBidQuoteAttachment_Update",
                p => new
                {
                    Id = p.Int(),
                    Comment = p.String(maxLength: 1024, unicode: false),
                    VendorId = p.Int(),
                    BidQuoteId = p.Int(),
                    AttachmentId = p.String(maxLength: 48, unicode: false),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[VendorBidQuoteAttachment]
                      SET [Comment] = @Comment, [VendorId] = @VendorId, [BidQuoteId] = @BidQuoteId, [AttachmentId] = @AttachmentId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorBidQuoteAttachment_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[VendorBidQuoteAttachment]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorInPart_Insert",
                p => new
                {
                    NotificationSent = p.DateTime(),
                    BidQuotePartId = p.Int(),
                    VendorId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[VendorInPart]([NotificationSent], [BidQuotePartId], [VendorId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@NotificationSent, @BidQuotePartId, @VendorId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorInPart]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[VendorInPart] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.VendorInPart_Update",
                p => new
                {
                    Id = p.Int(),
                    NotificationSent = p.DateTime(),
                    BidQuotePartId = p.Int(),
                    VendorId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[VendorInPart]
                      SET [NotificationSent] = @NotificationSent, [BidQuotePartId] = @BidQuotePartId, [VendorId] = @VendorId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorInPart_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[VendorInPart]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBidHistory_Insert",
                p => new
                {
                    QuantityAvailable = p.Int(),
                    ItemsPerUnit = p.Int(),
                    UnitPrice = p.Decimal(precision: 18, scale: 2),
                    EstimateDeliveryDate = p.DateTime(),
                    Comment = p.String(maxLength: 1024, unicode: false),
                    IsSelected = p.Boolean(),
                    LoggedInEmailAddress = p.String(unicode: false),
                    VendorPartBidId = p.Int(),
                    VendorPartBidNoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[VendorPartBidHistory]([QuantityAvailable], [ItemsPerUnit], [UnitPrice], [EstimateDeliveryDate], [Comment], [IsSelected], [LoggedInEmailAddress], [VendorPartBidId], [VendorPartBidNoteId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@QuantityAvailable, @ItemsPerUnit, @UnitPrice, @EstimateDeliveryDate, @Comment, @IsSelected, @LoggedInEmailAddress, @VendorPartBidId, @VendorPartBidNoteId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorPartBidHistory]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[VendorPartBidHistory] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBidHistory_Update",
                p => new
                {
                    Id = p.Int(),
                    QuantityAvailable = p.Int(),
                    ItemsPerUnit = p.Int(),
                    UnitPrice = p.Decimal(precision: 18, scale: 2),
                    EstimateDeliveryDate = p.DateTime(),
                    Comment = p.String(maxLength: 1024, unicode: false),
                    IsSelected = p.Boolean(),
                    LoggedInEmailAddress = p.String(unicode: false),
                    VendorPartBidId = p.Int(),
                    VendorPartBidNoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[VendorPartBidHistory]
                      SET [QuantityAvailable] = @QuantityAvailable, [ItemsPerUnit] = @ItemsPerUnit, [UnitPrice] = @UnitPrice, [EstimateDeliveryDate] = @EstimateDeliveryDate, [Comment] = @Comment, [IsSelected] = @IsSelected, [LoggedInEmailAddress] = @LoggedInEmailAddress, [VendorPartBidId] = @VendorPartBidId, [VendorPartBidNoteId] = @VendorPartBidNoteId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBidHistory_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[VendorPartBidHistory]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBidSelectedHistory_Insert",
                p => new
                {
                    VendorPartBidId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[VendorPartBidSelectedHistory]([VendorPartBidId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@VendorPartBidId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorPartBidSelectedHistory]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[VendorPartBidSelectedHistory] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBidSelectedHistory_Update",
                p => new
                {
                    Id = p.Int(),
                    VendorPartBidId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[VendorPartBidSelectedHistory]
                      SET [VendorPartBidId] = @VendorPartBidId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.VendorPartBidSelectedHistory_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[VendorPartBidSelectedHistory]
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuoteStatusHistory_Insert",
                p => new
                {
                    Status = p.String(maxLength: 24, unicode: false),
                    BidQuoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"INSERT [dbo].[BidQuoteStatusHistory]([Status], [BidQuoteId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@Status, @BidQuoteId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)

                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BidQuoteStatusHistory]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()

                      SELECT t0.[Id]
                      FROM [dbo].[BidQuoteStatusHistory] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            CreateStoredProcedure(
                "dbo.BidQuoteStatusHistory_Update",
                p => new
                {
                    Id = p.Int(),
                    Status = p.String(maxLength: 24, unicode: false),
                    BidQuoteId = p.Int(),
                    DomainKey = p.String(maxLength: 48, unicode: false),
                    DisplayOrder = p.Byte(),
                    IsDeleted = p.Boolean(),
                    DeletedBy = p.String(maxLength: 48, unicode: false),
                    DeletedOn = p.DateTime(),
                    IsActive = p.Boolean(),
                    CreatedBy = p.String(maxLength: 48, unicode: false),
                    CreatedOn = p.DateTime(),
                    UpdatedBy = p.String(maxLength: 48, unicode: false),
                    UpdatedOn = p.DateTime(),
                    DeactivatedBy = p.String(maxLength: 48, unicode: false),
                    DeactivatedOn = p.DateTime(),
                    Description = p.String(maxLength: 512, unicode: false),
                },
                body:
                    @"UPDATE [dbo].[BidQuoteStatusHistory]
                      SET [Status] = @Status, [BidQuoteId] = @BidQuoteId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "dbo.BidQuoteStatusHistory_Delete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"DELETE [dbo].[BidQuoteStatusHistory]
                      WHERE ([Id] = @Id)"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.BidQuoteStatusHistory_Delete");
            DropStoredProcedure("dbo.BidQuoteStatusHistory_Update");
            DropStoredProcedure("dbo.BidQuoteStatusHistory_Insert");
            DropStoredProcedure("dbo.VendorPartBidSelectedHistory_Delete");
            DropStoredProcedure("dbo.VendorPartBidSelectedHistory_Update");
            DropStoredProcedure("dbo.VendorPartBidSelectedHistory_Insert");
            DropStoredProcedure("dbo.VendorPartBidHistory_Delete");
            DropStoredProcedure("dbo.VendorPartBidHistory_Update");
            DropStoredProcedure("dbo.VendorPartBidHistory_Insert");
            DropStoredProcedure("dbo.VendorInPart_Delete");
            DropStoredProcedure("dbo.VendorInPart_Update");
            DropStoredProcedure("dbo.VendorInPart_Insert");
            DropStoredProcedure("dbo.VendorBidQuoteAttachment_Delete");
            DropStoredProcedure("dbo.VendorBidQuoteAttachment_Update");
            DropStoredProcedure("dbo.VendorBidQuoteAttachment_Insert");
            DropStoredProcedure("dbo.Contact_Delete");
            DropStoredProcedure("dbo.Contact_Update");
            DropStoredProcedure("dbo.Contact_Insert");
            DropStoredProcedure("dbo.Vendor_Delete");
            DropStoredProcedure("dbo.Vendor_Update");
            DropStoredProcedure("dbo.Vendor_Insert");
            DropStoredProcedure("dbo.VendorPartBid_Delete");
            DropStoredProcedure("dbo.VendorPartBid_Update");
            DropStoredProcedure("dbo.VendorPartBid_Insert");
            DropStoredProcedure("dbo.BidQuotePart_Delete");
            DropStoredProcedure("dbo.BidQuotePart_Update");
            DropStoredProcedure("dbo.BidQuotePart_Insert");
            DropStoredProcedure("dbo.BidQuote_Delete");
            DropStoredProcedure("dbo.BidQuote_Update");
            DropStoredProcedure("dbo.BidQuote_Insert");
            DropStoredProcedure("dbo.BidQuoteNote_Delete");
            DropStoredProcedure("dbo.BidQuoteNote_Update");
            DropStoredProcedure("dbo.BidQuoteNote_Insert");
            DropStoredProcedure("dbo.Attachment_Delete");
            DropStoredProcedure("dbo.Attachment_Update");
            DropStoredProcedure("dbo.Attachment_Insert");
            DropForeignKey("dbo.VendorBidQuoteAttachment", "BidQuoteId", "dbo.BidQuote");
            DropForeignKey("dbo.BidQuoteStatusHistory", "BidQuoteId", "dbo.BidQuote");
            DropForeignKey("dbo.BidQuotePart", "BidQuoteId", "dbo.BidQuote");
            DropForeignKey("dbo.VendorInPart", "BidQuotePartId", "dbo.BidQuotePart");
            DropForeignKey("dbo.VendorPartBid", "BidQuotePartId", "dbo.BidQuotePart");
            DropForeignKey("dbo.VendorPartBidSelectedHistory", "VendorPartBidId", "dbo.VendorPartBid");
            DropForeignKey("dbo.VendorPartBidHistory", "VendorPartBidId", "dbo.VendorPartBid");
            DropForeignKey("dbo.VendorInPart", "VendorId", "dbo.Vendor");
            DropForeignKey("dbo.VendorBidQuoteAttachment", "VendorId", "dbo.Vendor");
            DropForeignKey("dbo.VendorBidQuoteAttachment", "AttachmentId", "dbo.Attachment");
            DropForeignKey("dbo.Contact", "VendorId", "dbo.Vendor");
            DropForeignKey("dbo.VendorPartBid", "VendorId", "dbo.Vendor");
            DropForeignKey("dbo.BidQuoteNote", "BidQuoteId", "dbo.BidQuote");
            DropIndex("dbo.BidQuoteStatusHistory", new[] { "DomainKey" });
            DropIndex("dbo.BidQuoteStatusHistory", new[] { "BidQuoteId" });
            DropIndex("dbo.VendorPartBidSelectedHistory", new[] { "DomainKey" });
            DropIndex("dbo.VendorPartBidSelectedHistory", new[] { "VendorPartBidId" });
            DropIndex("dbo.VendorPartBidHistory", new[] { "DomainKey" });
            DropIndex("dbo.VendorPartBidHistory", new[] { "VendorPartBidId" });
            DropIndex("dbo.VendorInPart", new[] { "DomainKey" });
            DropIndex("dbo.VendorInPart", new[] { "VendorId" });
            DropIndex("dbo.VendorInPart", new[] { "BidQuotePartId" });
            DropIndex("dbo.VendorBidQuoteAttachment", new[] { "DomainKey" });
            DropIndex("dbo.VendorBidQuoteAttachment", new[] { "AttachmentId" });
            DropIndex("dbo.VendorBidQuoteAttachment", new[] { "BidQuoteId" });
            DropIndex("dbo.VendorBidQuoteAttachment", new[] { "VendorId" });
            DropIndex("dbo.Contact", new[] { "DomainKey" });
            DropIndex("dbo.Contact", new[] { "VendorId" });
            DropIndex("dbo.Vendor", new[] { "DomainKey" });
            DropIndex("dbo.VendorPartBid", new[] { "DomainKey" });
            DropIndex("dbo.VendorPartBid", new[] { "BidQuotePartId" });
            DropIndex("dbo.VendorPartBid", new[] { "VendorId" });
            DropIndex("dbo.BidQuotePart", new[] { "DomainKey" });
            DropIndex("dbo.BidQuotePart", new[] { "BidQuoteId" });
            DropIndex("dbo.BidQuote", new[] { "DomainKey" });
            DropIndex("dbo.BidQuoteNote", new[] { "DomainKey" });
            DropIndex("dbo.BidQuoteNote", new[] { "BidQuoteId" });
            DropTable("dbo.BidQuoteStatusHistory");
            DropTable("dbo.VendorPartBidSelectedHistory");
            DropTable("dbo.VendorPartBidHistory");
            DropTable("dbo.VendorInPart");
            DropTable("dbo.VendorBidQuoteAttachment");
            DropTable("dbo.Contact");
            DropTable("dbo.Vendor");
            DropTable("dbo.VendorPartBid");
            DropTable("dbo.BidQuotePart");
            DropTable("dbo.BidQuote");
            DropTable("dbo.BidQuoteNote");
            DropTable("dbo.Attachment");
        }
    }
}