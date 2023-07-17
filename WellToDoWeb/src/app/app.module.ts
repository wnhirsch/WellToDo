import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { CommonModule } from '@angular/common'
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms'

import { AppRoutingModule } from './app-routing.module'
import { TranslationsModule } from './translations.module'
import { AppComponent } from './app.component'
import { TaskListComponent } from './task-list/task-list.component';
import { MenuComponent } from './menu/menu.component';
import { GroupListComponent } from './group-list/group-list.component'

@NgModule({
  declarations: [
    AppComponent,
    TaskListComponent,
    MenuComponent,
    GroupListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    TranslationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
