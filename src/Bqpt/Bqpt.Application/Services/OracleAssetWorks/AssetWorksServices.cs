using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bqpt.Infrastructure;
using Dapper;

namespace Bqpt.Application
{
    public class AssetWorksServices : IAssetWorksServices
    {
        private readonly ISqlConnectionProvider _connectionProvider;

        public AssetWorksServices(ISqlConnectionProvider connectionProvider) => _connectionProvider = connectionProvider;

        public async Task<IEnumerable<AssetWorksBidQuoteDto>> GetAssetWorksBidQuotes()
        {
            using (var conn = _connectionProvider.GetDbOracleConnection("OracleAssetWorksDb"))
            {
                var sql = @"Select TRIM(NVL(rfxm.RFX_RFX_Id,''))  as AssetWorksBidQuoteId,
                                   rfxm.x_datetime_insert as AssetWorksDateInserted,
                                   rfxm.datetime_approval as AssetWorksDateApproval,
                                   rfxm.datetime_required as AssetWorksDateRequired,
                                   rfxm.datetime_requested as AssetWorksDateRequested,
                                   Count(Distinct parts.partno) as AssetWorksPartCount,
                                   Sum(CASE When parts.vmrs_code is null Then 1 Else 0 End) as AssetWorksPartCountWithNoCC,
                                   Sum(CASE When vendor1.venattr_attr_no is null and parts.vmrs_code is not null Then 1 Else 0 End) as AssetWorksNoVendorCCMatch,
                                   Count(Distinct vendor2.vend_vendor_no) as AssetWorksVendorsNoEmail

                              From EMSDBA.RFX_Main rfxm

                         Left Join (Select Concat(TRIM(NVL(rfxl.part_part_no,'')),TRIM(NVL(rfxl.PART_SUFFIX,''))) as partno,
                                           TRIM(NVL(rfxl.rfx_rfx_id,'')) as rfx_id,
                                           Max(ptsm.vmrs_code) as vmrs_code
                                     From EMSDBA.RFX_LINE rfxl
                                Left Join EMSDBA.pts_main ptsm
                                       On TRIM(NVL(rfxl.part_part_no,'')) = TRIM(NVL(ptsm.part_part_no,''))
                                      And TRIM(NVL(rfxl.PART_SUFFIX,'')) = TRIM(NVL(ptsm.PART_SUFFIX,''))
                                    Where NVL(rfxl.delete_row,'N') = 'N'
                                      And NVL(rfxl.PART_SUFFIX,'0') IN ('0','1')
                                      And NVL(rfxl.loc_loc_code,'') IN ('BT0001','BT0002')
                                    Group by TRIM(NVL(rfxl.rfx_rfx_id,'')), TRIM(NVL(rfxl.part_part_no,'')), TRIM(NVL(rfxl.PART_SUFFIX,''))) parts

                               On rfxm.RFX_RFX_Id = parts.rfx_id

                        Left Join (Select TRIM(NVL(va.venattr_attr_no,'')) as venattr_attr_no
                                     From EMSDBA.Ven_Main v
                                     Join EMSDBA.ven_attributes va
                                       On TRIM(NVL(v.vend_vendor_no,'')) = TRIM(NVL(va.vend_vendor_no,''))
                                      And TRIM(NVL(v.int_address3,'')) = 'TRANSIT BQP'
                                      And TRIM(NVL(v.Is_parts_vendor,'')) = 'Y'
                                      And NVL(va.delete_row,'N') = 'N'
                                      And NVL(v.is_active,'') = 'Y'
                                      And va.venattr_attr_no is not null
                                      And va.vend_vendor_no is not null
                                    Where v.email_addr is not null
                                 Group by va.venattr_attr_no) vendor1

                               On TRIM(NVL(parts.vmrs_code,'')) = vendor1.venattr_attr_no

                        Left Join (Select va.venattr_attr_no,
                                   v.vend_vendor_no
                                   From EMSDBA.Ven_Main v
                                   Join EMSDBA.ven_attributes va
                                   On TRIM(NVL(v.vend_vendor_no,'')) = TRIM(NVL(va.vend_vendor_no,''))
                                   And TRIM(NVL(v.int_address3,'')) = 'TRANSIT BQP'
                                   And TRIM(NVL(v.Is_parts_vendor,'')) = 'Y'
                                   And NVL(v.is_active,'') = 'Y'
                                   And va.venattr_attr_no is not null
                                   And va.vend_vendor_no is not null
                                   And NVL(va.delete_row,'N') = 'N'
                                   Where v.email_addr is null
                                   Group by va.venattr_attr_no, v.vend_vendor_no
                                   Order by va.venattr_attr_no, v.vend_vendor_no) vendor2

                               On TRIM(NVL(parts.vmrs_code,'')) = vendor2.venattr_attr_no

                             Where TRIM(NVL(rfxm.epstat_status_id,'')) = 'OPEN'
                               And TRIM(NVL(rfxm.eot_type_id,'')) = 'TRANSIT PO'

                          Group By rfxm.RFX_RFX_Id, rfxm.x_datetime_insert, rfxm.datetime_approval, rfxm.datetime_required, rfxm.datetime_requested
                          Order By rfxm.RFX_RFX_ID";

                var results = await conn.QueryAsync<AssetWorksBidQuoteDto>(sql);

                return results is null && !results.Any() ? Enumerable.Empty<AssetWorksBidQuoteDto>() : results;
            }
        }

        public async Task<AssetWorksBidQuoteDto> GetAssetWorksBidQuotesById(string assetWorksBidQuoteId)
        {
            using (var conn = _connectionProvider.GetDbOracleConnection("OracleAssetWorksDb"))
            {
                var sql = @"Select TRIM(NVL(rfxm.RFX_RFX_Id,''))  as AssetWorksBidQuoteId,
                                   rfxm.x_datetime_insert as AssetWorksDateInserted,
                                   rfxm.datetime_approval as AssetWorksDateApproval,
                                   rfxm.datetime_required as AssetWorksDateRequired,
                                   rfxm.datetime_requested as AssetWorksDateRequested

                              From EMSDBA.RFX_Main rfxm
                             Where rfxm.RFX_RFX_ID = :assetWorksBidQuoteId
                               And TRIM(NVL(rfxm.epstat_status_id,'')) = 'OPEN'
                               And TRIM(NVL(rfxm.eot_type_id,'')) = 'TRANSIT PO'

                          Group By rfxm.RFX_RFX_Id, rfxm.x_datetime_insert, rfxm.datetime_approval, rfxm.datetime_required, rfxm.datetime_requested
                          Order By rfxm.RFX_RFX_ID";

                var results = await conn.QueryFirstOrDefaultAsync<AssetWorksBidQuoteDto>(sql, new { assetWorksBidQuoteId });

                return results is null ? new AssetWorksBidQuoteDto() : results;
            }
        }

        public async Task<IEnumerable<AssetWorksBidQuotePartDto>> GetAssetWorksBidQuoteParts(string assetWorksBidQuoteId)
        {
            using (var conn = _connectionProvider.GetDbOracleConnection("OracleAssetWorksDb"))
            {
                var sql = @"Select TRIM(NVL(rfxl.RFX_RFX_ID,''))  as AssetWorksBidQuoteId,
                                   TRIM(NVL(rfxl.PART_PART_NO,''))  as AssetWorksPartNumber,
                                   Max(TRIM(NVL(ptsm.VMRS_CODE,''))) as AssetWorksPartCommodityCode,
                                   Max(TRIM(NVL(plom.MANUF_PART_NO,''))) as AssetWorksPartManufacturerPartNumber,
                                   TRIM(NVL(rfxl.PART_SUFFIX,'0')) as AssetWorksPartSuffix,
                                   Max(TRIM(NVL(rfxl.PART_DESCRIPTION,''))) as AssetWorksPartDescription,
                                   Max(TRIM(NVL(rfxl.Unit_Price,''))) as AssetWorksUnitPrice,
                                   Min(rfxl.LINE_NO) as AssetWorksMinPartLineNumber,

                                   LISTAGG(DISTINCT(TRIM(NVL(rfxl.PRD_PRODUCT_CATEGORY,''))), ',') WITHIN GROUP (ORDER BY rfxl.PRD_PRODUCT_CATEGORY) as AssetWorksPartCategory,
                                   LISTAGG(DISTINCT(TRIM(NVL(rfxl.LINE_NO,''))), ',') WITHIN GROUP (ORDER BY rfxl.LINE_NO) as AssetWorksPartLineNumber,
                                   LISTAGG(DISTINCT(TRIM(NVL(rfxl.LOC_LOC_CODE,''))), ',') WITHIN GROUP (ORDER BY rfxl.LOC_LOC_CODE) as AssetWorksPartLocationCode,

                                   Min(rfxl.X_DATETIME_INSERT) as AssetWorksPartDateInserted,
                                   Min(rfxl.DATETIME_REQUIRED) as AssetWorksPartDateRequired,
                                   Sum(TO_NUMBER(rfxl.QTY)) as AssetWorksPartQuantityRequested,

                                   Max(plo_tot.QTY_ON_HAND) as AssetWorksPartQuantityOnHand,
                                   Max(plo_tot.QTY_ON_ORDER) as AssetWorksPartQuantityOnOrder,
                                   Max(plo_tot.QTY_COMMITTED) as AssetWorksPartQuantityCommited

                              From EMSDBA.RFX_LINE rfxl
                         Left join EMSDBA.PLO_MAIN plom
                                ON plom.PART_PART_NO = rfxl.PART_PART_NO
                               And plom.PART_SUFFIX = rfxl.PART_SUFFIX
                               And plom.LOC_LOC_CODE = rfxl.LOC_LOC_CODE

                         Left Join EMSDBA.V_PLO_STOCK_TOTALS plo_tot
                                ON plo_tot.PART_PART_NO = rfxl.PART_PART_NO
                               And plo_tot.PART_SUFFIX = rfxl.PART_SUFFIX

                         Left Join EMSDBA.pts_main ptsm
                                On ptsm.PART_PART_NO = rfxl.PART_PART_NO
                               And NVL(ptsm.PART_SUFFIX,'0') = NVL(rfxl.PART_SUFFIX,'0')

                              Join (Select TRIM(NVL(va.venattr_attr_no,'')) as venattr_attr_no
                                   From EMSDBA.ven_attributes va
                                   Join EMSDBA.Ven_Main v
                                   On v.vend_vendor_no is not null
                                   And v.vend_vendor_no = va.vend_vendor_no
                                   And TRIM(NVL(v.int_address3,'')) = 'TRANSIT BQP'
                                   And TRIM(NVL(v.Is_parts_vendor,'')) = 'Y'
                                   And NVL(va.delete_row,'N') = 'N'
                                   And NVL(v.is_active,'') = 'Y'
                                   Where va.venattr_attr_no is not null
                                   And va.vend_vendor_no is not null
                                   And v.email_addr Is Not Null
                                   Group by va.venattr_attr_no) vendor
                                   On TRIM(NVL(ptsm.VMRS_CODE,'')) = vendor.venattr_attr_no

                             Where rfxl.RFX_RFX_ID = :assetWorksBidQuoteId
                               And NVL(rfxl.delete_row,'N') = 'N'
                               And NVL(rfxl.PART_SUFFIX,'0') IN ('0','1')
                               And NVL(rfxl.loc_loc_code,'') IN ('BT0001','BT0002')

                          Group By rfxl.RFX_RFX_ID,
                                   rfxl.PART_PART_NO,
                                   NVL(rfxl.PART_SUFFIX,'0')

                          Order By Min(rfxl.LINE_NO), rfxl.PART_PART_NO, TRIM(NVL(rfxl.PART_SUFFIX,'0'))";

                var results = await conn.QueryAsync<AssetWorksBidQuotePartDto>(sql, new { assetWorksBidQuoteId });

                return results is null && !results.Any() ? Enumerable.Empty<AssetWorksBidQuotePartDto>() : results;
            }
        }

        public async Task<IEnumerable<AssetWorksBidQuotePartDto>> GetAssetWorksBidQuotePartsAll(string assetWorksBidQuoteId)
        {
            using (var conn = _connectionProvider.GetDbOracleConnection("OracleAssetWorksDb"))
            {
                var sql = @"Select TRIM(NVL(rfxl.RFX_RFX_ID,''))  as AssetWorksBidQuoteId,
                                   TRIM(NVL(rfxl.PART_PART_NO,''))  as AssetWorksPartNumber,
                                   Max(TRIM(NVL(ptsm.VMRS_CODE,''))) as AssetWorksPartCommodityCode,
                                   Max(TRIM(NVL(plom.MANUF_PART_NO,''))) as AssetWorksPartManufacturerPartNumber,
                                   TRIM(NVL(rfxl.PART_SUFFIX,'0')) as AssetWorksPartSuffix,
                                   Max(TRIM(NVL(rfxl.PART_DESCRIPTION,''))) as AssetWorksPartDescription,
                                   Max(TRIM(NVL(rfxl.Unit_Price,''))) as AssetWorksUnitPrice,
                                   Min(rfxl.LINE_NO) as AssetWorksMinPartLineNumber,

                                   LISTAGG(DISTINCT(TRIM(NVL(rfxl.PRD_PRODUCT_CATEGORY,''))), ',') WITHIN GROUP (ORDER BY rfxl.PRD_PRODUCT_CATEGORY) as AssetWorksPartCategory,
                                   LISTAGG(DISTINCT(TRIM(NVL(rfxl.LINE_NO,''))), ',') WITHIN GROUP (ORDER BY rfxl.LINE_NO) as AssetWorksPartLineNumber,
                                   LISTAGG(DISTINCT(TRIM(NVL(rfxl.LOC_LOC_CODE,''))), ',') WITHIN GROUP (ORDER BY rfxl.LOC_LOC_CODE) as AssetWorksPartLocationCode,

                                   Min(rfxl.X_DATETIME_INSERT) as AssetWorksPartDateInserted,
                                   Min(rfxl.DATETIME_REQUIRED) as AssetWorksPartDateRequired,
                                   Sum(TO_NUMBER(rfxl.QTY)) as AssetWorksPartQuantityRequested,

                                   Max(plo_tot.QTY_ON_HAND) as AssetWorksPartQuantityOnHand,
                                   Max(plo_tot.QTY_ON_ORDER) as AssetWorksPartQuantityOnOrder,
                                   Max(plo_tot.QTY_COMMITTED) as AssetWorksPartQuantityCommited

                              From EMSDBA.RFX_LINE rfxl
                         Left join EMSDBA.PLO_MAIN plom
                                ON plom.PART_PART_NO = rfxl.PART_PART_NO
                               And plom.PART_SUFFIX = rfxl.PART_SUFFIX
                               And plom.LOC_LOC_CODE = rfxl.LOC_LOC_CODE

                         Left Join EMSDBA.V_PLO_STOCK_TOTALS plo_tot
                                ON plo_tot.PART_PART_NO = rfxl.PART_PART_NO
                               And plo_tot.PART_SUFFIX = rfxl.PART_SUFFIX

                         Left Join EMSDBA.pts_main ptsm
                                On ptsm.PART_PART_NO = rfxl.PART_PART_NO
                               And NVL(ptsm.PART_SUFFIX,'0') = NVL(rfxl.PART_SUFFIX,'0')

                             Where rfxl.RFX_RFX_ID = :assetWorksBidQuoteId
                               And NVL(rfxl.delete_row,'N') = 'N'
                               And NVL(rfxl.PART_SUFFIX,'0') IN ('0','1')
                               And NVL(rfxl.loc_loc_code,'') IN ('BT0001','BT0002')

                          Group By rfxl.RFX_RFX_ID,
                                   rfxl.PART_PART_NO,
                                   NVL(rfxl.PART_SUFFIX,'0')

                          Order By Min(rfxl.LINE_NO), rfxl.PART_PART_NO, TRIM(NVL(rfxl.PART_SUFFIX,'0'))";

                var results = await conn.QueryAsync<AssetWorksBidQuotePartDto>(sql, new { assetWorksBidQuoteId });

                return results is null && !results.Any() ? Enumerable.Empty<AssetWorksBidQuotePartDto>() : results;
            }
        }

        public async Task<IEnumerable<AssetWorksVendorDto>> GetAssetWorksVendors()
        {
            using (var conn = _connectionProvider.GetDbOracleConnection("OracleAssetWorksDb"))
            {
                var sql = @"Select LISTAGG(DISTINCT(NVL(va.venattr_attr_no,'')), ',') WITHIN GROUP (ORDER BY va.venattr_attr_no) as CommodityCodes,
                                   Trim(NVL(v.vend_vendor_no,''))  as AssetWorksVendorNumber,
                                   Trim(NVL(v.accounting_sys_no,''))  as AssetWorksVendorAccountingSystemNumber,
                                   Trim(NVL(v.name,''))  as AssetWorksVendorName,
                                   Trim(NVL(v.contact_name,'')) as AssetWorksVendorContactName,
                                   Trim(NVL(v.address1,''))  as AssetWorksVendorAddress1,
                                   Trim(NVL(v.address2,''))  as AssetWorksVendorAddress2,
                                   Trim(NVL(v.address3,''))  as AssetWorksVendorAddress3,
                                   Trim(NVL(v.address4,''))  as AssetWorksVendorAddress4,
                                   Lower(Trim(NVL(v.email_addr,''))) as AssetWorksVendorEmail,
                                   Trim(NVL(v.phone,'')) as AssetWorksVendorPhone,
                                   Trim(NVL(v.fax,'')) as AssetWorksVendorFax

                              From EMSDBA.Ven_Main v
                        Inner Join EMSDBA.ven_attributes va
                                On v.vend_vendor_no = va.vend_vendor_no
                             Where TRIM(NVL(v.int_address3,'')) = 'TRANSIT BQP'
                               And NVL(v.Is_parts_vendor,'') = 'Y'
                               And NVL(v.is_active,'') = 'Y'
                               And va.venattr_attr_no Is Not Null
                               And NVL(va.delete_row,'N') = 'N'
                          Group by v.vend_vendor_no, v.accounting_sys_no, v.name, v.contact_name, v.address1, v.address2, v.address3, v.address4, v.email_addr, v.phone, v.fax, v.is_active
                          Order by v.vend_vendor_no";

                var results = await conn.QueryAsync<AssetWorksVendorDto>(sql);

                return results is null && !results.Any() ? Enumerable.Empty<AssetWorksVendorDto>() : results;
            }
        }

        public async Task<IEnumerable<AssetWorksVendorContactDto>> GetAssetWorksVendorContacts()
        {
            using (var conn = _connectionProvider.GetDbOracleConnection("OracleAssetWorksDb"))
            {
                var sql = @"Select Trim(v.name) as AssetWorksVendorName,
                                  Trim(vc.vend_vendor_no) as AssetWorksVendorNumber,
                                   Trim(vc.Contact_Name) as AssetWorksContactName,
                                   Trim(vc.Phone) as AssetWorksContactPhone,
                                   Lower(Trim(vc.Email_Addr)) as AssetWorksContactEmail,
                                   Trim(vc.Address1) as AssetWorksContactAddress1,
                                   Trim(vc.Address2) as AssetWorksContactAddress2,
                                   Trim(vc.Address3) as AssetWorksContactAddress3,
                                   Trim(vc.Address4) as AssetWorksContactAddress4

                              From EMSDBA.ven_contact vc
                        Inner Join EMSDBA.Ven_Main v
                                On v.vend_vendor_no = vc.vend_vendor_no
                               And TRIM(NVL(v.int_address3,'')) = 'TRANSIT BQP'
                               And NVL(v.Is_parts_vendor,'') = 'Y'
                               And NVL(v.is_active,'') = 'Y'
                        Inner Join EMSDBA.ven_attributes va
                                On v.vend_vendor_no = va.vend_vendor_no
                               And va.venattr_attr_no Is Not Null
                               And NVL(va.delete_row,'N') = 'N'

                             Where vencnt_contact_type_id  = 'TRANSIT-PARTS'
                               And NVL(vc.delete_row,'N') = 'N'
                               And vc.x_datetime_insert = (Select Max(vc2.x_datetime_insert) From ven_contact vc2 Where Trim(vc2.email_addr) = Trim(vc.email_addr))
                          Group by Trim(v.name),
                                   Trim(vc.vend_vendor_no),
                                   Trim(vc.Contact_Name),
                                   Trim(vc.Phone),
                                   Lower(Trim(vc.Email_Addr)),
                                   Trim(vc.Address1),
                                   Trim(vc.Address2),
                                   Trim(vc.Address3),
                                   Trim(vc.Address4)";

                var results = await conn.QueryAsync<AssetWorksVendorContactDto>(sql);

                return results is null && !results.Any() ? Enumerable.Empty<AssetWorksVendorContactDto>() : results;
            }
        }
    }
}