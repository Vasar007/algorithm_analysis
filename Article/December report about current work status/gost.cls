\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{gost}[2015/10/04 GOST R 7.0.11-2011]

\LoadClassWithOptions{scrartcl}

\KOMAoptions{%
  toc=sectionentrydotfill,
  bibliography=totoc,
  paper=a4,footinclude=false,headinclude=false,
}

% page margins
\RequirePackage{typearea}
\areaset[15mm]{175mm}{257mm}

% line and paragraph spacing
\RequirePackage{setspace}
\setstretch{1.5}
\setlength{\parskip}{0pt}
\setlength{\parindent}{2.5em}

% page numbering
\RequirePackage{scrlayer-scrpage}
\cohead{}
\cofoot{\thepage\vfill}
\setkomafont{pagefoot}{\scriptsize\mdseries\upshape}
\pagestyle{scrheadings}

% section format and spacing
% N.B. We use 2 baseline skip here because we have one and half spacing
\RedeclareSectionCommand[%
    font=\LARGE\singlespacing\centering,%
    beforeskip=2\baselineskip,%
    afterskip=2\baselineskip%
]{section}

\RedeclareSectionCommand[%
    font=\Large\singlespacing\centering,%
    beforeskip=2\baselineskip,%
    afterskip=2\baselineskip%
]{subsection}

\RedeclareSectionCommand[%
    font=\large\singlespacing\centering,%
    beforeskip=2\baselineskip,%
    afterskip=2\baselineskip%
]{subsubsection}

\RedeclareSectionCommand[%
    font=\normalsize\bfseries,%
    beforeskip=0pt,%
    indent=2.5em
]{paragraph}

% additional fields for title page
\newcommand*{\organization}[1]{\gdef\@organization{#1}}
\newcommand*{\manuscript}[1]{\gdef\@manuscript{#1}}
\newcommand*{\speciality}[1]{\gdef\@speciality{#1}}
\newcommand*{\degree}[1]{\gdef\@degree{#1}}
\newcommand*{\supervisor}[1]{\gdef\@supervisor{#1}}

\newcommand*{\@organization}[1]{organization is not set}
\newcommand*{\@manuscript}[1]{manuscript is not set}
\newcommand*{\@speciality}[1]{speciality is not set}
\newcommand*{\@degree}[1]{degree is not set}
\newcommand*{\@supervisor}[1]{supervisor is not set}

% title page template
\renewcommand*{\maketitle}{%
\begin{titlepage}%
  \centering%
  \begin{spacing}{1}%
    {\normalsize\@organization\par}\vspace{1cm}%
    \begin{flushright}%
	  {\normalsize\itshape\@manuscript\par}%
    \end{flushright}%
	\vspace{2cm}%
    {\large\@degree\par}\vspace{3cm}%
    {\huge\bfseries\@title\par}\vspace{1cm}%
    {\Large\@author\par}\vspace{3cm}%
    {\normalsize\@speciality\par}\vspace{1cm}%
    \begin{flushright}%
      {\normalsize\@supervisor\par}%
    \end{flushright}%
    \vfill%
    {\normalsize\@date\par}%
  \end{spacing}%
\end{titlepage}%
}

% title page numbering
\newcommand{\maketitlepage}{%
  \pagenumbering{gobble}%
  \thispagestyle{empty}%
  \maketitle%
  \clearpage%
  \pagenumbering{arabic}%
  \setcounter{page}{2}%
}

%\AtBeginDocument{\maketitlepage}

% clear page before each section
\RequirePackage{etoolbox}
\pretocmd{\section}{\clearpage}{}{}

\RequirePackage{hyperref}
