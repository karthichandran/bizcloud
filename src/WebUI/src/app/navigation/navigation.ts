import { FuseNavigation } from '@fuse/types';

export const navigation: FuseNavigation[] = [
  {
    id: 'setup',
    title: 'SETUP',
    translate: 'NAV.SETUP',
    type: 'collapsable',
    children: [
      //{
      //  id: 'sample',
      //  title: 'Sample',
      //  translate: 'NAV.SAMPLE.TITLE',
      //  type: 'item',
      //  icon: 'account_balance',
      //  url: '/sample',
      //  badge: {
      //    title: '25',
      //    translate: 'NAV.SAMPLE.BADGE',
      //    bg: '#F44336',
      //    fg: '#FFFFFF'
      //  }
      //},
      {
        id: 'user',
        title: 'Users',
        translate: 'NAV.USER.TITLE',
        type: 'item',
        icon: 'account_box',
        url: '/user'
      },
      {
        id: 'userRole',
        title: 'User Role',
        translate: 'NAV.USERROLE.TITLE',
        type: 'item',
        icon: 'how_to_reg',
        url: '/role'
      },
      {
        id: 'rolePermissions',
        title: 'Role Permissions',
        translate: 'NAV.ROLEPERMISSIONS.TITLE',
        type: 'item',
        icon: 'settings',
        url: '/role-permissions'
      },
      {
        id: 'tax',
        title: 'Tax',
        translate: 'NAV.TAX.TITLE',
        type: 'item',
        icon: 'receipt',
        url: '/tax'
      },
      {
        id: 'seller',
        title: 'Seller',
        translate: 'NAV.SELLER.TITLE',
        type: 'item',
        icon: 'location_city',
        url: '/seller'

      },
      {
        id: 'property',
        title: 'Property',
        translate: 'NAV.PROPERTY.TITLE',
        type: 'item',
        icon: 'business',
        url: '/property'
      },
      {
        id: 'accounts',
        title: 'Bank Accounts',
        translate: 'NAV.PROPERTY.TITLE',
        type: 'item',
        icon: 'account_balance',
        url: '/bank-accounts'
      }
      //{
      //  id: 'tdsReceipt',
      //  title: 'Tds Receipt',
      //  translate: 'NAV.TDSRECEIPT.TITLE',
      //  type: 'item',
      //  icon: 'receipt',
      //  url: '/lot'
      //},     
    ]
  },
  {
    id: 'customer',
    title: 'CUSTOMER',
    translate: 'NAV.CUSTOMER',
    type: 'collapsable',
    children: [
      {
        id: 'add_customer',
        title: 'Customer Details',
        translate: 'NAV.ADD_CUSTOMER.TITLE',
        type: 'item',
        icon: 'people',
        url: '/client'
      },
      {
        id: 'customer_billing',
        title: 'Customer Billing',
        translate: 'NAV.CUSTOMERBILLING.TITLE',
        type: 'item',
        icon: 'ballot',
        url: '/customer-billing'
      },
      {
        id: 'pre_sales',
        title: 'Pre Sales',
        translate: 'NAV.PRESALES.TITLE',
        type: 'item',
        icon: 'local_offer',
        url: '/pre-sales'
      }

    ]
  },
  {
    id: 'client',
    title: 'CLIENT',
    translate: 'NAV.CLIENT',
    type: 'collapsable',
    children: [
      {
        id: 'client_Payment',
        title: 'Client Payment',
        translate: 'NAV.CLIENTPAYMENT.TITLE',
        type: 'item',
        icon: 'payment',
        url: '/client-payment'
      }, {
        id: 'tdsRemitance',
        title: 'TDS Remittance',
        translate: 'NAV.TDSREMEITANCE.TITLE',
        type: 'item',
        icon: 'list_alt',
        url: '/tds-remitance'
      }, {
        id: 'tdsReceipt',
        title: 'Receipt',
        translate: 'NAV.TDSRECEIPT.TITLE',
        type: 'item',
        icon: 'receipt',
        url: '/tds-receipt'
      }]
  },
  {
    id: 'reports',
    title: 'REPORTS',
    translate: 'NAV.REPORTS',
    type: 'collapsable',
    children: [
      {
        id: 'status_report',
        title: 'Status Report',
        translate: 'NAV.STATUSREPORT.TITLE',
        type: 'item',
        icon: 'assignment',
        url: '/status-report'
      },
      {
        id: 'seller_compliance_report',
        title: 'Seller Compliance Report',
        translate: 'NAV.SELLERCOMPREPORT.TITLE',
        type: 'item',
        icon: 'bar_chart',
        url: '/seller-compliance-report'
      },
      {
        id: 'tds_remittance_report',
        title: 'TDS Remittance Report',
        translate: 'NAV.TDSREMITTANCEREPORT.TITLE',
        type: 'item',
        icon: 'network_check',
        url: '/tds-remittance-report'
      },
      {
        id: 'lot_summary_report',
        title: 'Lot Summary Report',
        translate: 'NAV.LOTSUMMARTREPORT.TITLE',
        type: 'item',
        icon: 'assessment',
        url: '/lot-summary-report'
      },
      {
        id: 'statement_of_account',
        title: 'Statement Of Account',
        translate: 'NAV.ACCOUNTSTATEMENT.TITLE',
        type: 'item',
        icon: 'account_balance',
        url: '/statement-of-account-report'
      },
      {
        id: 'password_setting_report',
        title: 'Password Setting Report',
        translate: 'NAV.PASSWORDSETTING.TITLE',
        type: 'item',
        icon: 'vpn_key',
        url: '/password-setting-report'
      },{
        id: 'tax_payment_report',
        title: 'Tax Payment Report',
        translate: 'NAV.TAXPAYMENT.TITLE',
        type: 'item',
        icon: 'payment',
        url: '/tax-payment-report'
      }

    ]
  }

];
