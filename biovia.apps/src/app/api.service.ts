import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { Project } from './project';
import { Study } from './study';
import { Experiment } from './experiment';

const httpOptions = {
  headers: new HttpHeaders({'Content-Type': 'application/json'})
};
const apiUrl = "https://localhost:5001/api/projects";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getProjects (): Observable<Project[]> {
    return this.http.get<Project[]>(apiUrl)
      .pipe(
        tap(projects => console.log('Fetch projects')),
        catchError(this.handleError('getProjects', []))
      );
  }

  getStudies (proj_id: string): Observable<Study[]> {
    const url = `${apiUrl}/${proj_id}/studies`;
    return this.http.get<Study[]>(url)
      .pipe(
        tap(studies => console.log('Fetch studies')),
        catchError(this.handleError('getStudies', []))
      );
  }

  getExperiments (proj_id: string, study_id: string): Observable<Experiment[]> {
    const url = `${apiUrl}/${proj_id}/studies/${study_id}/experiments`;
    return this.http.get<Experiment[]>(url)
      .pipe(
        tap(studies => console.log('Fetch experiments')),
        catchError(this.handleError('getExperiments', []))
      );
  }

  getStudy (proj_id: string, study_id: string): Observable<Study> {
    const url = `${apiUrl}/${proj_id}/studies/${study_id}`;
    return this.http.get<Study>(url)
    .pipe(
      tap((study: any) => console.log('Fetch study')),
      catchError(this.handleError('getStudy', []))
    );
  }

  getProject(id: number): Observable<Project> {
    const url = `${apiUrl}/${id}`;
    return this.http.get<Project>(url).pipe(
      tap(_ => console.log(`fetched project id=${id}`)),
      catchError(this.handleError<Project>(`getProject id=${id}`))
    );
  }

  addProject (project): Observable<Project> {
    return this.http.post<Project>(apiUrl, project, httpOptions).pipe(
      tap((project: Project) => console.log(`added project w/ id=${project.proj_id}`)),
      catchError(this.handleError<Project>('addProject'))
    );
  }

  updateProject (id, project): Observable<any> {
    const url = `${apiUrl}/${id}`;
    return this.http.put(url, project, httpOptions).pipe(
      tap(_ => console.log(`updated project id=${id}`)),
      catchError(this.handleError<any>('updateProject'))
    );
  }

  updateExperimentOrder (proj_id: string, study_id: string, sort_col: string, sort_order: string): Observable<any> {
    const url = `${apiUrl}/${proj_id}/studies/${study_id}/experiments`;
    let sortReq = {
      StudyId: study_id,
      SortColumn: sort_col,
      SortOrder: sort_order
    }
    return this.http.post<Project>(url, sortReq, httpOptions).pipe(
      tap((project: Project) => console.log(`updated sort for experiments for study id=${study_id}`)),
      catchError(this.handleError<Project>('updateExperimentOrder'))
    );
  }

  deleteProject (id): Observable<Project> {
    const url = `${apiUrl}/${id}`;

    return this.http.delete<Project>(url, httpOptions).pipe(
      tap(_ => console.log(`deleted project id=${id}`)),
      catchError(this.handleError<Project>('deleteProject'))
    );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
