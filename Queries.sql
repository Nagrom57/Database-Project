

	select Project.Movies.MovieID, Title, Project.Languages.LanguageID, Language
	from Project.Movies join Project.MovieLanguages
			on Project.Movies.MovieID = Project.MovieLanguages.MovieID
		join Project.Languages
			on Project.Languages.LanguageID = Project.MovieLanguages.LanguageID