export interface IPropertyDto {
  propertyID: number;
  propertyType: number;
  addressPremises?: string | undefined;
  addressLine1?: string | undefined;
  addressLine2?: string | undefined;
  city?: string | undefined;
  pinCode?: string | undefined;
  tdsInterestRate: number;
  lateFeePerDay?: number;
  stateID: number;
  gstTaxCode: number;
  tDSTaxCode: number;
  isActive: boolean;
}

export interface ISellerPropertyDto {
  sellerPropertyId: number;
  sellerShare: number;
  propertyID_FK?: number | undefined;
  sellerID: number | undefined;
}

export interface IPropertyVM {
  propertyDto?: IPropertyDto | undefined;
  sellerProperties?: ISellerPropertyDto[] | undefined;
}
