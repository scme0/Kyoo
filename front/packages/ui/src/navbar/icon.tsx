/*
 * Kyoo - A portable and vast media library solution.
 * Copyright (c) Kyoo.
 *
 * See AUTHORS.md and LICENSE file in the project root for full license information.
 *
 * Kyoo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * Kyoo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Kyoo. If not, see <https://www.gnu.org/licenses/>.
 */

import Svg, { SvgProps, Path } from "react-native-svg";

/* export const KyooLogo = (props: SvgProps) => ( */
/* 	<Svg viewBox="0 0 128 128" {...props}> */
/* 		<Path */
/* 			d="M121.39 62.34 6.5 8.58v107.51z" */
/* 			fill="#121327" */
/* 			stroke="#010202" */
/* 			strokeWidth={5} */
/* 			strokeMiterlimit={10} */
/* 		/> */
/* 		<Path */
/* 			d="M63.09 25.59c-4.59 8.24-9.01 17.92-13.23 29.04-3.65 9.52-6.53 18.78-8.64 27.78-1.42 6.05-2.06 9.83-1.91 11.32.22.58.33 1.11.33 1.59 0 .77-.31 1.15-.93 1.15-.77 0-2.01-.66-3.72-1.97-1.9-1.42-2.84-2.59-2.84-3.5 0-.58.16-1.44.49-2.57 2.99-10.54 6.71-21.69 11.16-33.47 2.92-7.29 6.09-15.24 9.52-23.84-10.43 2.55-17.88 5.61-22.37 9.19-1.64 1.28-2.46 2.5-2.46 3.66 0 .8.36 2.94 1.09 6.4.25 1.13-.15 1.7-1.2 1.7-.73 0-1.29-.35-1.7-1.04-1.24-1.97-2.52-4.83-3.83-8.59-.26-.84-.38-1.6-.38-2.3 0-2.73 2.11-4.96 6.34-6.67 1.49-.62 9.04-3.17 22.64-7.66 3.21-1.06 5.4-2.31 6.56-3.77.84-1.02 1.57-1.53 2.19-1.53.58 0 1.31.33 2.19.98 1.02.73 1.53 1.35 1.53 1.86-.01.48-.29 1.22-.83 2.24zm32.21-4.37c.73.73 1.09 1.33 1.09 1.8 0 .58-.77 1.82-2.3 3.72-4.01 4.96-9.92 11.28-17.72 18.98-7.47 7.36-12.54 11.94-15.2 13.73 3.32 4.7 6.63 9.42 9.95 14.16 5.07 7.15 9.44 11.94 13.13 14.38 1.6 1.09 3.04 1.64 4.32 1.64 1.28 0 2.77-.62 4.48-1.86.36-.25.71-.38 1.04-.38.77 0 1.15.38 1.15 1.15 0 .44-.16.89-.49 1.37-1.09 1.71-3.48 4.21-7.16 7.49-.84.77-1.66 1.15-2.46 1.15-.95 0-2.02-.49-3.23-1.48-4.48-3.57-9.52-9.28-15.09-17.12-5.83-8.2-9.88-13.29-12.14-15.26-1.17.47-2.1.71-2.79.71-1.28 0-1.99-.95-2.13-2.84-.04-.58-.05-1.08-.05-1.48 0-1.53.41-2.65 1.23-3.36s2.49-1.34 5-1.89c.91-.18 1.6.05 2.08.71 4.3-3.32 9.17-7.78 14.6-13.4 5.36-5.5 8.6-9.37 9.73-11.59-2.3-1.35-3.45-2.46-3.45-3.34 0-.69.66-1.18 1.97-1.48 1.82-1.28 3.99-3.34 6.51-6.18 1.06-1.2 2.19-1.8 3.39-1.8 1.38.01 2.9.83 4.54 2.47z" */
/* 			fill="#e13e13" */
/* 		/> */
/* 	</Svg> */
/* ); */

export const KyooLongLogo = ({
	height = 24,
	...props
}: Omit<SvgProps, "width" | "height"> & { height?: number }) => (
	<Svg viewBox="49.954 131.833 318.13 108.676" height={height} width={height * 2.9272} {...props}>
		<Path
			d="m164.844 186.759-114.89-53.76v107.51l114.89-53.75Z"
			fill="#121327"
			stroke="#010202"
			strokeWidth={5}
			strokeMiterlimit={10}
		/>
		<Path
			d="M106.544 150.009c-4.59 8.24-9.01 17.92-13.23 29.04-3.65 9.52-6.53 18.78-8.64 27.78-1.42 6.05-2.06 9.83-1.91 11.32.22.58.33 1.11.33 1.59 0 .77-.31 1.15-.93 1.15-.77 0-2.01-.66-3.72-1.97-1.9-1.42-2.84-2.59-2.84-3.5 0-.58.16-1.44.49-2.57 2.99-10.54 6.71-21.69 11.16-33.47 2.92-7.29 6.09-15.24 9.52-23.84-10.43 2.55-17.88 5.61-22.37 9.19-1.64 1.28-2.46 2.5-2.46 3.66 0 .8.36 2.94 1.09 6.4.25 1.13-.15 1.7-1.2 1.7-.73 0-1.29-.35-1.7-1.04-1.24-1.97-2.52-4.83-3.83-8.59-.26-.84-.38-1.6-.38-2.3 0-2.73 2.11-4.96 6.34-6.67 1.49-.62 9.04-3.17 22.64-7.66 3.21-1.06 5.4-2.31 6.56-3.77.84-1.02 1.57-1.53 2.19-1.53.58 0 1.31.33 2.19.98 1.02.73 1.53 1.35 1.53 1.86-.01.48-.29 1.22-.83 2.24Zm32.21-4.37c.73.73 1.09 1.33 1.09 1.8 0 .58-.77 1.82-2.3 3.72-4.01 4.96-9.92 11.28-17.72 18.98-7.47 7.36-12.54 11.94-15.2 13.73 3.32 4.7 6.63 9.42 9.95 14.16 5.07 7.15 9.44 11.94 13.13 14.38 1.6 1.09 3.04 1.64 4.32 1.64 1.28 0 2.77-.62 4.48-1.86.36-.25.71-.38 1.04-.38.77 0 1.15.38 1.15 1.15 0 .44-.16.89-.49 1.37-1.09 1.71-3.48 4.21-7.16 7.49-.84.77-1.66 1.15-2.46 1.15-.95 0-2.02-.49-3.23-1.48-4.48-3.57-9.52-9.28-15.09-17.12-5.83-8.2-9.88-13.29-12.14-15.26-1.17.47-2.1.71-2.79.71-1.28 0-1.99-.95-2.13-2.84-.04-.58-.05-1.08-.05-1.48 0-1.53.41-2.65 1.23-3.36.82-.71 2.49-1.34 5-1.89.91-.18 1.6.05 2.08.71 4.3-3.32 9.17-7.78 14.6-13.4 5.36-5.5 8.6-9.37 9.73-11.59-2.3-1.35-3.45-2.46-3.45-3.34 0-.69.66-1.18 1.97-1.48 1.82-1.28 3.99-3.34 6.51-6.18 1.06-1.2 2.19-1.8 3.39-1.8 1.38.01 2.9.83 4.54 2.47Z"
			fill="#e13e13"
		/>
		<Path
			d="m213.414 212.784-18.219-23.932v23.932h-13.202V158.59h13.202v23.777l18.064-23.777h15.518l-20.999 26.556 21.771 27.638Z"
			fill="#121327"
			strokeWidth={0}
		/>
		<Path
			d="m271.063 158.59-18.759 36.284v17.91h-13.202v-17.91l-18.759-36.284h14.977l10.499 22.696 10.422-22.696Z"
			fill="#121327"
			strokeWidth={0}
		/>
		<Path
			d="M290.814 213.324q-7.642 0-14.011-3.551-6.369-3.551-10.114-9.92-3.744-6.369-3.744-14.321 0-7.951 3.744-14.282 3.745-6.33 10.114-9.881 6.369-3.551 14.011-3.551 7.643 0 14.012 3.551 6.369 3.551 10.036 9.881 3.667 6.331 3.667 14.282 0 7.952-3.705 14.321-3.706 6.369-10.036 9.92-6.331 3.551-13.974 3.551Zm0-12.043q6.485 0 10.384-4.323 3.898-4.323 3.898-11.426 0-7.179-3.898-11.464-3.899-4.284-10.384-4.284-6.562 0-10.46 4.246-3.899 4.246-3.899 11.502 0 7.18 3.899 11.465 3.898 4.284 10.46 4.284Z"
			fill="#121327"
			strokeWidth={0}
		/>
		<Path
			d="M340.285 213.324q-7.643 0-14.012-3.551-6.369-3.551-10.113-9.92-3.744-6.369-3.744-14.321 0-7.951 3.744-14.282 3.744-6.33 10.113-9.881t14.012-3.551q7.643 0 14.012 3.551 6.369 3.551 10.036 9.881Q368 177.581 368 185.532q0 7.952-3.706 14.321-3.705 6.369-10.036 9.92-6.33 3.551-13.973 3.551Zm0-12.043q6.485 0 10.383-4.323 3.899-4.323 3.899-11.426 0-7.179-3.899-11.464-3.898-4.284-10.383-4.284-6.562 0-10.461 4.246-3.898 4.246-3.898 11.502 0 7.18 3.898 11.465 3.899 4.284 10.461 4.284Z"
			fill="#121327"
			strokeWidth={0}
		/>
	</Svg>
);