
export interface ICustomer {
  customerID: number;
  name: string | undefined;
  pAN: string | undefined;
  addressPremises: string | undefined;
  adressLine1: string | undefined;
  addressLine2: string | undefined;
  city: string | undefined;
  pinCode: string | undefined;
  mobileNo: string | undefined;
  emailID: string | undefined;
  dateOfBirth: Date;
  stateId: number;
  isTracesRegistered: boolean;
  tracesPassword: string | undefined;
  tracesLogin: string | undefined;
  allowForm16B: boolean;
  customerProperty?: ICustomerProperty[] | undefined;
}

export interface ICustomerProperty {
  customerPropertyId: number;
  customerShare: number;
  customerId: number;
  propertyId: number;
  dateOfSubmission: Date;
  unitNo: number;
  remarks: string | undefined;
  isShared: boolean;
  statusTypeId: number;
  paymentMethodId: number;
  gstrate: number;
  tdsrate: number;
  totalUnitCost: number;
  agreementDate: Date; 
  tdsCollectedBySeller: boolean;
  dateOfAgreement: Date;
}


export interface ICustomerShareList {
  customerID?: number;
  name: string | undefined;
  share: string | undefined;
  form16b?: number ;
}

export interface ICustomerVM {
  customers ?: ICustomer [] | undefined;
  customerProperty?: ICustomerProperty[] | undefined;
}
