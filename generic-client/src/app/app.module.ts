import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TableViewComponent } from './pages/home/components/table-view/table-view.component';
import { HttpClientModule } from '@angular/common/http';
import { GetValuePipe } from './pages/home/utils/getValue';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material-module';
import { TreeViewComponent } from './pages/home/components/tree-view/tree-view.component';
import { ModelSelectorComponent } from './pages/home/components/model-selector/model-selector.component';
import { ParseNamePipe } from './pages/home/utils/parseName';
import { SidebarComponent } from './pages/home/components/sidebar/sidebar.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TableViewComponent,
    GetValuePipe,
    ParseNamePipe,
    TreeViewComponent,
    ModelSelectorComponent,
    SidebarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
