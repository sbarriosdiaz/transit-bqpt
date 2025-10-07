namespace Bqpt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntChangeBidQuote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidQuotePart", "AssetWorksMinPartLineNumberInt", c => c.Int(nullable: false));
            //DropColumn("dbo.BidQuotePart", "MinPartLineNumberInt");
            AlterStoredProcedure(
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
                        AssetWorksMinPartLineNumberInt = p.Int(),
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
                    @"INSERT [dbo].[BidQuotePart]([AssetWorksPartNumber], [AssetWorksPartCommodityCode], [AssetWorksPartManufacturerPartNumber], [AssetWorksPartSuffix], [AssetWorksPartDescription], [AssetWorksUnitPrice], [AssetWorksPartCategory], [AssetWorksPartLineNumber], [AssetWorksMinPartLineNumber], [AssetWorksMinPartLineNumberInt], [AssetWorksPartLocationCode], [AssetWorksPartDateInserted], [AssetWorksPartDateRequired], [AssetWorksPartQuantityRequested], [AssetWorksPartQuantityOnHand], [AssetWorksPartQuantityOnOrder], [AssetWorksPartQuantityCommited], [RequestedQuantity], [PurchaseOrderQuantity], [BidQuoteId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@AssetWorksPartNumber, @AssetWorksPartCommodityCode, @AssetWorksPartManufacturerPartNumber, @AssetWorksPartSuffix, @AssetWorksPartDescription, @AssetWorksUnitPrice, @AssetWorksPartCategory, @AssetWorksPartLineNumber, @AssetWorksMinPartLineNumber, @AssetWorksMinPartLineNumberInt, @AssetWorksPartLocationCode, @AssetWorksPartDateInserted, @AssetWorksPartDateRequired, @AssetWorksPartQuantityRequested, @AssetWorksPartQuantityOnHand, @AssetWorksPartQuantityOnOrder, @AssetWorksPartQuantityCommited, @RequestedQuantity, @PurchaseOrderQuantity, @BidQuoteId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BidQuotePart]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[BidQuotePart] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
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
                        AssetWorksMinPartLineNumberInt = p.Int(),
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
                      SET [AssetWorksPartNumber] = @AssetWorksPartNumber, [AssetWorksPartCommodityCode] = @AssetWorksPartCommodityCode, [AssetWorksPartManufacturerPartNumber] = @AssetWorksPartManufacturerPartNumber, [AssetWorksPartSuffix] = @AssetWorksPartSuffix, [AssetWorksPartDescription] = @AssetWorksPartDescription, [AssetWorksUnitPrice] = @AssetWorksUnitPrice, [AssetWorksPartCategory] = @AssetWorksPartCategory, [AssetWorksPartLineNumber] = @AssetWorksPartLineNumber, [AssetWorksMinPartLineNumber] = @AssetWorksMinPartLineNumber, [AssetWorksMinPartLineNumberInt] = @AssetWorksMinPartLineNumberInt, [AssetWorksPartLocationCode] = @AssetWorksPartLocationCode, [AssetWorksPartDateInserted] = @AssetWorksPartDateInserted, [AssetWorksPartDateRequired] = @AssetWorksPartDateRequired, [AssetWorksPartQuantityRequested] = @AssetWorksPartQuantityRequested, [AssetWorksPartQuantityOnHand] = @AssetWorksPartQuantityOnHand, [AssetWorksPartQuantityOnOrder] = @AssetWorksPartQuantityOnOrder, [AssetWorksPartQuantityCommited] = @AssetWorksPartQuantityCommited, [RequestedQuantity] = @RequestedQuantity, [PurchaseOrderQuantity] = @PurchaseOrderQuantity, [BidQuoteId] = @BidQuoteId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            //AddColumn("dbo.BidQuotePart", "MinPartLineNumberInt", c => c.Int(nullable: false));
            DropColumn("dbo.BidQuotePart", "AssetWorksMinPartLineNumberInt");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
