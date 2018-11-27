import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectsComponent } from './projects/projects.component';
import { StudiesComponent } from './studies/studies.component';
import { ExperimentsComponent } from './experiments/experiments.component';

const routes: Routes = [
  {
    path: 'projects',
    component: ProjectsComponent,
    data: { title: 'List of Projects' }
  },
  {
    path: 'projects/:id/studies',
    component: StudiesComponent,
    data: { title: 'List of Study' }
  },
  {
    path: 'projects/:pid/studies/:sid/experiments',
    component: ExperimentsComponent,
    data: { title: 'List of Experiments' }
  },
  { path: '',
    redirectTo: '/projects',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
