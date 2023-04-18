// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  
   baseApiUrl: 'http://localhost:44301',  // For connecting to server running elsewhere update the base API URL
   serverUrl: 'https://localhost:44301',
  //baseApiUrl: 'http://leansyshost-001-site3.itempurl.com',
  //serverUrl: 'http://leansyshost-001-site3.itempurl.com',
  oauth: {
    enabled: false,  // For connecting to Mifos X using OAuth2 Authentication change the value to true
    serverUrl: ''
  }
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
