export class CustomerPropertyDto {
  public customerPropertyID: number;
  public customerShare: number;
  public customerID: number;
  public propertyID: number;
  public dateOfSubmission: Date;
  public unitNo: number;
  public remarks: string;
  public paymentMethod: number;
  public isShared: boolean;
  public statusTypeID: number;
  public paymentMethodID: number;
  public gSTRate: number;
  public tDSRate: number;
  public totalUnitCost: number;
  public agreementDate: Date;
  public allowForm16B: boolean;
  public tdsCollectedBySeller: boolean;
}
