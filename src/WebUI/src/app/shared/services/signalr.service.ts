// import { Injectable } from '@angular/core';
// import * as signalR from "@aspnet/signalr";
// import { environment } from 'environments/environment';
// import { ToastrService } from 'ngx-toastr';
// @Injectable({
//   providedIn: 'root'
// })
// export class SignalRService {


//   private hubConnection: signalR.HubConnection

//   constructor(private toastr:ToastrService) { }


//   public startConnection = () => {
//     //let tokenObj: any = {};
//     //if (localStorage.getItem("auth_token") != "" && localStorage.getItem("auth_token") != null)
//     //  tokenObj = JSON.parse(localStorage.getItem("auth_token")); 
//     //this.hubConnection = new signalR.HubConnectionBuilder()
//     //  .withUrl(environment.serverUrl + '/broadcast', { accessTokenFactory: () => tokenObj.token})
//     //  .build();
//     this.hubConnection = new signalR.HubConnectionBuilder()
//       .withUrl(environment.serverUrl + '/broadcast')
//       .build();

//     this.hubConnection
//       .start()
//       .then(() => console.log('Connection started'))
//       .catch(err => console.log('Error while starting connection: ' + err))
//   }   

//   public addBroadcastDataListener = () => {
//     this.hubConnection.on('broadcastTest', (data) => {
//       //this.toastr.success(" broadcast : "+data);
//       console.log(data);
//     })
//   }
// }
