import { Component, inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { PhotoManagement } from "./photo-management/photo-management";

@Component({
  selector: 'app-admin',
  imports: [PhotoManagement],
  templateUrl: './admin.html',
  styleUrl: './admin.css',
})
export class Admin {
  protected accountService = inject(AccountService);
  activeTab = 'photos';
  tabs = [
    { name: 'Photo moderation', value: 'photos' },
    { name: 'Users management', value: 'roles' },    
  ];

  setTab(tab: string) {
    this.activeTab = tab;
  }

}
