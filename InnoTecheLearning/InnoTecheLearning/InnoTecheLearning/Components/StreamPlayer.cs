using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using System;
using Android.Net;
using Android.Media;
using Java.IO;
using Xamarin.Forms;
using Stream = System.IO.Stream;
using System.Linq;
using Math = Java.Lang.Math;
using IEnumerator = System.Collections.IEnumerator;
using IEnumerable = System.Collections.IEnumerable;
#elif NETFX_CORE
using System;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Streams;
//using static Windows.ApplicationModel.Package;
#endif

namespace InnoTecheLearning
{
    public partial class Utils
    {
        /// <summary>
        /// A <see cref="StreamPlayer"/> that plays streams.
        /// </summary>
        public class StreamPlayer : ISoundPlayer
        {
            public class StreamPlayerOptions
            {
                #region Mime Types
                public static Dictionary<string, string> MimeTypes = new Dictionary<string, string>
        {
            { "123", "application/vnd.lotus-1-2-3" },
            { "3dml", "text/vnd.in3d.3dml" },
            { "3g2", "video/3gpp2" },
            { "3gp", "video/3gpp" },
            { "7z", "application/x-7z-compressed" },
            { "aab", "application/x-authorware-bin" },
            { "aac", "audio/x-aac" },
            { "aam", "application/x-authorware-map" },
            { "aas", "application/x-authorware-seg" },
            { "abw", "application/x-abiword" },
            { "ac", "application/pkix-attr-cert" },
            { "acc", "application/vnd.americandynamics.acc" },
            { "ace", "application/x-ace-compressed" },
            { "acu", "application/vnd.acucobol" },
            { "acutc", "application/vnd.acucorp" },
            { "adp", "audio/adpcm" },
            { "aep", "application/vnd.audiograph" },
            { "afm", "application/x-font-type1" },
            { "afp", "application/vnd.ibm.modcap" },
            { "ahead", "application/vnd.ahead.space" },
            { "ai", "application/postscript" },
            { "aif", "audio/x-aiff" },
            { "aifc", "audio/x-aiff" },
            { "aiff", "audio/x-aiff" },
            { "air", "application/vnd.adobe.air-application-installer-package+zip" },
            { "ait", "application/vnd.dvb.ait" },
            { "ami", "application/vnd.amiga.ami" },
            { "apk", "application/vnd.android.package-archive" },
            { "application", "application/x-ms-application" },
            { "apr", "application/vnd.lotus-approach" },
            { "asc", "application/pgp-signature" },
            { "asf", "video/x-ms-asf" },
            { "asm", "text/x-asm" },
            { "aso", "application/vnd.accpac.simply.aso" },
            { "asx", "video/x-ms-asf" },
            { "atc", "application/vnd.acucorp" },
            { "atom", "application/atom+xml" },
            { "atomcat", "application/atomcat+xml" },
            { "atomsvc", "application/atomsvc+xml" },
            { "atx", "application/vnd.antix.game-component" },
            { "au", "audio/basic" },
            { "avi", "video/x-msvideo" },
            { "aw", "application/applixware" },
            { "azf", "application/vnd.airzip.filesecure.azf" },
            { "azs", "application/vnd.airzip.filesecure.azs" },
            { "azw", "application/vnd.amazon.ebook" },
            { "bat", "application/x-msdownload" },
            { "bcpio", "application/x-bcpio" },
            { "bdf", "application/x-font-bdf" },
            { "bdm", "application/vnd.syncml.dm+wbxml" },
            { "bed", "application/vnd.realvnc.bed" },
            { "bh2", "application/vnd.fujitsu.oasysprs" },
            { "bin", "application/octet-stream" },
            { "bmi", "application/vnd.bmi" },
            { "bmp", "image/bmp" },
            { "book", "application/vnd.framemaker" },
            { "box", "application/vnd.previewsystems.box" },
            { "boz", "application/x-bzip2" },
            { "bpk", "application/octet-stream" },
            { "btif", "image/prs.btif" },
            { "bz", "application/x-bzip" },
            { "bz2", "application/x-bzip2" },
            { "c", "text/x-c" },
            { "c11amc", "application/vnd.cluetrust.cartomobile-config" },
            { "c11amz", "application/vnd.cluetrust.cartomobile-config-pkg" },
            { "c4d", "application/vnd.clonk.c4group" },
            { "c4f", "application/vnd.clonk.c4group" },
            { "c4g", "application/vnd.clonk.c4group" },
            { "c4p", "application/vnd.clonk.c4group" },
            { "c4u", "application/vnd.clonk.c4group" },
            { "cab", "application/vnd.ms-cab-compressed" },
            { "car", "application/vnd.curl.car" },
            { "cat", "application/vnd.ms-pki.seccat" },
            { "cc", "text/x-c" },
            { "cct", "application/x-director" },
            { "ccxml", "application/ccxml+xml" },
            { "cdbcmsg", "application/vnd.contact.cmsg" },
            { "cdf", "application/x-netcdf" },
            { "cdkey", "application/vnd.mediastation.cdkey" },
            { "cdmia", "application/cdmi-capability" },
            { "cdmic", "application/cdmi-container" },
            { "cdmid", "application/cdmi-domain" },
            { "cdmio", "application/cdmi-object" },
            { "cdmiq", "application/cdmi-queue" },
            { "cdx", "chemical/x-cdx" },
            { "cdxml", "application/vnd.chemdraw+xml" },
            { "cdy", "application/vnd.cinderella" },
            { "cer", "application/pkix-cert" },
            { "cgm", "image/cgm" },
            { "chat", "application/x-chat" },
            { "chm", "application/vnd.ms-htmlhelp" },
            { "chrt", "application/vnd.kde.kchart" },
            { "cif", "chemical/x-cif" },
            { "cii", "application/vnd.anser-web-certificate-issue-initiation" },
            { "cil", "application/vnd.ms-artgalry" },
            { "cla", "application/vnd.claymore" },
            { "class", "application/java-vm" },
            { "clkk", "application/vnd.crick.clicker.keyboard" },
            { "clkp", "application/vnd.crick.clicker.palette" },
            { "clkt", "application/vnd.crick.clicker.template" },
            { "clkw", "application/vnd.crick.clicker.wordbank" },
            { "clkx", "application/vnd.crick.clicker" },
            { "clp", "application/x-msclip" },
            { "cmc", "application/vnd.cosmocaller" },
            { "cmdf", "chemical/x-cmdf" },
            { "cml", "chemical/x-cml" },
            { "cmp", "application/vnd.yellowriver-custom-menu" },
            { "cmx", "image/x-cmx" },
            { "cod", "application/vnd.rim.cod" },
            { "com", "application/x-msdownload" },
            { "conf", "text/plain" },
            { "cpio", "application/x-cpio" },
            { "cpp", "text/x-c" },
            { "cpt", "application/mac-compactpro" },
            { "crd", "application/x-mscardfile" },
            { "crl", "application/pkix-crl" },
            { "crt", "application/x-x509-ca-cert" },
            { "cryptonote", "application/vnd.rig.cryptonote" },
            { "csh", "application/x-csh" },
            { "csml", "chemical/x-csml" },
            { "csp", "application/vnd.commonspace" },
            { "css", "text/css" },
            { "cst", "application/x-director" },
            { "csv", "text/csv" },
            { "cu", "application/cu-seeme" },
            { "curl", "text/vnd.curl" },
            { "cww", "application/prs.cww" },
            { "cxt", "application/x-director" },
            { "cxx", "text/x-c" },
            { "dae", "model/vnd.collada+xml" },
            { "daf", "application/vnd.mobius.daf" },
            { "dataless", "application/vnd.fdsn.seed" },
            { "davmount", "application/davmount+xml" },
            { "dcr", "application/x-director" },
            { "dcurl", "text/vnd.curl.dcurl" },
            { "dd2", "application/vnd.oma.dd2+xml" },
            { "ddd", "application/vnd.fujixerox.ddd" },
            { "deb", "application/x-debian-package" },
            { "def", "text/plain" },
            { "deploy", "application/octet-stream" },
            { "der", "application/x-x509-ca-cert" },
            { "dfac", "application/vnd.dreamfactory" },
            { "dic", "text/x-c" },
            { "dir", "application/x-director" },
            { "dis", "application/vnd.mobius.dis" },
            { "dist", "application/octet-stream" },
            { "distz", "application/octet-stream" },
            { "djv", "image/vnd.djvu" },
            { "djvu", "image/vnd.djvu" },
            { "dll", "application/x-msdownload" },
            { "dmg", "application/octet-stream" },
            { "dms", "application/octet-stream" },
            { "dna", "application/vnd.dna" },
            { "doc", "application/msword" },
            { "docm", "application/vnd.ms-word.document.macroenabled.12" },
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "dot", "application/msword" },
            { "dotm", "application/vnd.ms-word.template.macroenabled.12" },
            { "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
            { "dp", "application/vnd.osgi.dp" },
            { "dpg", "application/vnd.dpgraph" },
            { "dra", "audio/vnd.dra" },
            { "dsc", "text/prs.lines.tag" },
            { "dssc", "application/dssc+der" },
            { "dtb", "application/x-dtbook+xml" },
            { "dtd", "application/xml-dtd" },
            { "dts", "audio/vnd.dts" },
            { "dtshd", "audio/vnd.dts.hd" },
            { "dump", "application/octet-stream" },
            { "dvi", "application/x-dvi" },
            { "dwf", "model/vnd.dwf" },
            { "dwg", "image/vnd.dwg" },
            { "dxf", "image/vnd.dxf" },
            { "dxp", "application/vnd.spotfire.dxp" },
            { "dxr", "application/x-director" },
            { "ecelp4800", "audio/vnd.nuera.ecelp4800" },
            { "ecelp7470", "audio/vnd.nuera.ecelp7470" },
            { "ecelp9600", "audio/vnd.nuera.ecelp9600" },
            { "ecma", "application/ecmascript" },
            { "edm", "application/vnd.novadigm.edm" },
            { "edx", "application/vnd.novadigm.edx" },
            { "efif", "application/vnd.picsel" },
            { "ei6", "application/vnd.pg.osasli" },
            { "elc", "application/octet-stream" },
            { "eml", "message/rfc822" },
            { "emma", "application/emma+xml" },
            { "eol", "audio/vnd.digital-winds" },
            { "eot", "application/vnd.ms-fontobject" },
            { "eps", "application/postscript" },
            { "epub", "application/epub+zip" },
            { "es3", "application/vnd.eszigno3+xml" },
            { "esf", "application/vnd.epson.esf" },
            { "et3", "application/vnd.eszigno3+xml" },
            { "etx", "text/x-setext" },
            { "exe", "application/x-msdownload" },
            { "exi", "application/exi" },
            { "ext", "application/vnd.novadigm.ext" },
            { "ez", "application/andrew-inset" },
            { "ez2", "application/vnd.ezpix-album" },
            { "ez3", "application/vnd.ezpix-package" },
            { "f", "text/x-fortran" },
            { "f4v", "video/x-f4v" },
            { "f77", "text/x-fortran" },
            { "f90", "text/x-fortran" },
            { "fbs", "image/vnd.fastbidsheet" },
            { "fcs", "application/vnd.isac.fcs" },
            { "fdf", "application/vnd.fdf" },
            { "fe_launch", "application/vnd.denovo.fcselayout-link" },
            { "fg5", "application/vnd.fujitsu.oasysgp" },
            { "fgd", "application/x-director" },
            { "fh", "image/x-freehand" },
            { "fh4", "image/x-freehand" },
            { "fh5", "image/x-freehand" },
            { "fh7", "image/x-freehand" },
            { "fhc", "image/x-freehand" },
            { "fig", "application/x-xfig" },
            { "fli", "video/x-fli" },
            { "flo", "application/vnd.micrografx.flo" },
            { "flv", "video/x-flv" },
            { "flw", "application/vnd.kde.kivio" },
            { "flx", "text/vnd.fmi.flexstor" },
            { "fly", "text/vnd.fly" },
            { "fm", "application/vnd.framemaker" },
            { "fnc", "application/vnd.frogans.fnc" },
            { "for", "text/x-fortran" },
            { "fpx", "image/vnd.fpx" },
            { "frame", "application/vnd.framemaker" },
            { "fsc", "application/vnd.fsc.weblaunch" },
            { "fst", "image/vnd.fst" },
            { "ftc", "application/vnd.fluxtime.clip" },
            { "fti", "application/vnd.anser-web-funds-transfer-initiation" },
            { "fvt", "video/vnd.fvt" },
            { "fxp", "application/vnd.adobe.fxp" },
            { "fxpl", "application/vnd.adobe.fxp" },
            { "fzs", "application/vnd.fuzzysheet" },
            { "g2w", "application/vnd.geoplan" },
            { "g3", "image/g3fax" },
            { "g3w", "application/vnd.geospace" },
            { "gac", "application/vnd.groove-account" },
            { "gdl", "model/vnd.gdl" },
            { "geo", "application/vnd.dynageo" },
            { "gex", "application/vnd.geometry-explorer" },
            { "ggb", "application/vnd.geogebra.file" },
            { "ggt", "application/vnd.geogebra.tool" },
            { "ghf", "application/vnd.groove-help" },
            { "gif", "image/gif" },
            { "gim", "application/vnd.groove-identity-message" },
            { "gmx", "application/vnd.gmx" },
            { "gnumeric", "application/x-gnumeric" },
            { "gph", "application/vnd.flographit" },
            { "gqf", "application/vnd.grafeq" },
            { "gqs", "application/vnd.grafeq" },
            { "gram", "application/srgs" },
            { "gre", "application/vnd.geometry-explorer" },
            { "grv", "application/vnd.groove-injector" },
            { "grxml", "application/srgs+xml" },
            { "gsf", "application/x-font-ghostscript" },
            { "gtar", "application/x-gtar" },
            { "gtm", "application/vnd.groove-tool-message" },
            { "gtw", "model/vnd.gtw" },
            { "gv", "text/vnd.graphviz" },
            { "gxt", "application/vnd.geonext" },
            { "h", "text/x-c" },
            { "h261", "video/h261" },
            { "h263", "video/h263" },
            { "h264", "video/h264" },
            { "hal", "application/vnd.hal+xml" },
            { "hbci", "application/vnd.hbci" },
            { "hdf", "application/x-hdf" },
            { "hh", "text/x-c" },
            { "hlp", "application/winhlp" },
            { "hpgl", "application/vnd.hp-hpgl" },
            { "hpid", "application/vnd.hp-hpid" },
            { "hps", "application/vnd.hp-hps" },
            { "hqx", "application/mac-binhex40" },
            { "htke", "application/vnd.kenameaapp" },
            { "htm", "text/html" },
            { "html", "text/html" },
            { "hvd", "application/vnd.yamaha.hv-dic" },
            { "hvp", "application/vnd.yamaha.hv-voice" },
            { "hvs", "application/vnd.yamaha.hv-script" },
            { "i2g", "application/vnd.intergeo" },
            { "icc", "application/vnd.iccprofile" },
            { "ice", "x-conference/x-cooltalk" },
            { "icm", "application/vnd.iccprofile" },
            { "ico", "image/x-icon" },
            { "ics", "text/calendar" },
            { "ief", "image/ief" },
            { "ifb", "text/calendar" },
            { "ifm", "application/vnd.shana.informed.formdata" },
            { "iges", "model/iges" },
            { "igl", "application/vnd.igloader" },
            { "igm", "application/vnd.insors.igm" },
            { "igs", "model/iges" },
            { "igx", "application/vnd.micrografx.igx" },
            { "iif", "application/vnd.shana.informed.interchange" },
            { "imp", "application/vnd.accpac.simply.imp" },
            { "ims", "application/vnd.ms-ims" },
            { "in", "text/plain" },
            { "ipfix", "application/ipfix" },
            { "ipk", "application/vnd.shana.informed.package" },
            { "irm", "application/vnd.ibm.rights-management" },
            { "irp", "application/vnd.irepository.package+xml" },
            { "iso", "application/octet-stream" },
            { "itp", "application/vnd.shana.informed.formtemplate" },
            { "ivp", "application/vnd.immervision-ivp" },
            { "ivu", "application/vnd.immervision-ivu" },
            { "jad", "text/vnd.sun.j2me.app-descriptor" },
            { "jam", "application/vnd.jam" },
            { "jar", "application/java-archive" },
            { "java", "text/x-java-source" },
            { "jisp", "application/vnd.jisp" },
            { "jlt", "application/vnd.hp-jlyt" },
            { "jnlp", "application/x-java-jnlp-file" },
            { "joda", "application/vnd.joost.joda-archive" },
            { "jpe", "image/jpeg" },
            { "jpeg", "image/jpeg" },
            { "jpg", "image/jpeg" },
            { "jpgm", "video/jpm" },
            { "jpgv", "video/jpeg" },
            { "jpm", "video/jpm" },
            { "js", "application/javascript" },
            { "json", "application/json" },
            { "kar", "audio/midi" },
            { "karbon", "application/vnd.kde.karbon" },
            { "kfo", "application/vnd.kde.kformula" },
            { "kia", "application/vnd.kidspiration" },
            { "kml", "application/vnd.google-earth.kml+xml" },
            { "kmz", "application/vnd.google-earth.kmz" },
            { "kne", "application/vnd.kinar" },
            { "knp", "application/vnd.kinar" },
            { "kon", "application/vnd.kde.kontour" },
            { "kpr", "application/vnd.kde.kpresenter" },
            { "kpt", "application/vnd.kde.kpresenter" },
            { "ksp", "application/vnd.kde.kspread" },
            { "ktr", "application/vnd.kahootz" },
            { "ktx", "image/ktx" },
            { "ktz", "application/vnd.kahootz" },
            { "kwd", "application/vnd.kde.kword" },
            { "kwt", "application/vnd.kde.kword" },
            { "lasxml", "application/vnd.las.las+xml" },
            { "latex", "application/x-latex" },
            { "lbd", "application/vnd.llamagraphics.life-balance.desktop" },
            { "lbe", "application/vnd.llamagraphics.life-balance.exchange+xml" },
            { "les", "application/vnd.hhe.lesson-player" },
            { "lha", "application/octet-stream" },
            { "link66", "application/vnd.route66.link66+xml" },
            { "list", "text/plain" },
            { "list3820", "application/vnd.ibm.modcap" },
            { "listafp", "application/vnd.ibm.modcap" },
            { "log", "text/plain" },
            { "lostxml", "application/lost+xml" },
            { "lrf", "application/octet-stream" },
            { "lrm", "application/vnd.ms-lrm" },
            { "ltf", "application/vnd.frogans.ltf" },
            { "lvp", "audio/vnd.lucent.voice" },
            { "lwp", "application/vnd.lotus-wordpro" },
            { "lzh", "application/octet-stream" },
            { "m13", "application/x-msmediaview" },
            { "m14", "application/x-msmediaview" },
            { "m1v", "video/mpeg" },
            { "m21", "application/mp21" },
            { "m2a", "audio/mpeg" },
            { "m2v", "video/mpeg" },
            { "m3a", "audio/mpeg" },
            { "m3u", "audio/x-mpegurl" },
            { "m3u8", "application/vnd.apple.mpegurl" },
            { "m4u", "video/vnd.mpegurl" },
            { "m4v", "video/x-m4v" },
            { "ma", "application/mathematica" },
            { "mads", "application/mads+xml" },
            { "mag", "application/vnd.ecowin.chart" },
            { "maker", "application/vnd.framemaker" },
            { "man", "text/troff" },
            { "mathml", "application/mathml+xml" },
            { "mb", "application/mathematica" },
            { "mbk", "application/vnd.mobius.mbk" },
            { "mbox", "application/mbox" },
            { "mc1", "application/vnd.medcalcdata" },
            { "mcd", "application/vnd.mcd" },
            { "mcurl", "text/vnd.curl.mcurl" },
            { "mdb", "application/x-msaccess" },
            { "mdi", "image/vnd.ms-modi" },
            { "me", "text/troff" },
            { "mesh", "model/mesh" },
            { "meta4", "application/metalink4+xml" },
            { "mets", "application/mets+xml" },
            { "mfm", "application/vnd.mfmp" },
            { "mgp", "application/vnd.osgeo.mapguide.package" },
            { "mgz", "application/vnd.proteus.magazine" },
            { "mid", "audio/midi" },
            { "midi", "audio/midi" },
            { "mif", "application/vnd.mif" },
            { "mime", "message/rfc822" },
            { "mj2", "video/mj2" },
            { "mjp2", "video/mj2" },
            { "mlp", "application/vnd.dolby.mlp" },
            { "mmd", "application/vnd.chipnuts.karaoke-mmd" },
            { "mmf", "application/vnd.smaf" },
            { "mmr", "image/vnd.fujixerox.edmics-mmr" },
            { "mny", "application/x-msmoney" },
            { "mobi", "application/x-mobipocket-ebook" },
            { "mods", "application/mods+xml" },
            { "mov", "video/quicktime" },
            { "movie", "video/x-sgi-movie" },
            { "mp2", "audio/mpeg" },
            { "mp21", "application/mp21" },
            { "mp2a", "audio/mpeg" },
            { "mp3", "audio/mpeg" },
            { "mp4", "video/mp4" },
            { "mp4a", "audio/mp4" },
            { "mp4s", "application/mp4" },
            { "mp4v", "video/mp4" },
            { "mpc", "application/vnd.mophun.certificate" },
            { "mpe", "video/mpeg" },
            { "mpeg", "video/mpeg" },
            { "mpg", "video/mpeg" },
            { "mpg4", "video/mp4" },
            { "mpga", "audio/mpeg" },
            { "mpkg", "application/vnd.apple.installer+xml" },
            { "mpm", "application/vnd.blueice.multipass" },
            { "mpn", "application/vnd.mophun.application" },
            { "mpp", "application/vnd.ms-project" },
            { "mpt", "application/vnd.ms-project" },
            { "mpy", "application/vnd.ibm.minipay" },
            { "mqy", "application/vnd.mobius.mqy" },
            { "mrc", "application/marc" },
            { "mrcx", "application/marcxml+xml" },
            { "ms", "text/troff" },
            { "mscml", "application/mediaservercontrol+xml" },
            { "mseed", "application/vnd.fdsn.mseed" },
            { "mseq", "application/vnd.mseq" },
            { "msf", "application/vnd.epson.msf" },
            { "msh", "model/mesh" },
            { "msi", "application/x-msdownload" },
            { "msl", "application/vnd.mobius.msl" },
            { "msty", "application/vnd.muvee.style" },
            { "mts", "model/vnd.mts" },
            { "mus", "application/vnd.musician" },
            { "musicxml", "application/vnd.recordare.musicxml+xml" },
            { "mvb", "application/x-msmediaview" },
            { "mwf", "application/vnd.mfer" },
            { "mxf", "application/mxf" },
            { "mxl", "application/vnd.recordare.musicxml" },
            { "mxml", "application/xv+xml" },
            { "mxs", "application/vnd.triscape.mxs" },
            { "mxu", "video/vnd.mpegurl" },
            { "n3", "text/n3" },
            { "nb", "application/mathematica" },
            { "nbp", "application/vnd.wolfram.player" },
            { "nc", "application/x-netcdf" },
            { "ncx", "application/x-dtbncx+xml" },
            { "n-gage", "application/vnd.nokia.n-gage.symbian.install" },
            { "ngdat", "application/vnd.nokia.n-gage.data" },
            { "nlu", "application/vnd.neurolanguage.nlu" },
            { "nml", "application/vnd.enliven" },
            { "nnd", "application/vnd.noblenet-directory" },
            { "nns", "application/vnd.noblenet-sealer" },
            { "nnw", "application/vnd.noblenet-web" },
            { "npx", "image/vnd.net-fpx" },
            { "nsf", "application/vnd.lotus-notes" },
            { "oa2", "application/vnd.fujitsu.oasys2" },
            { "oa3", "application/vnd.fujitsu.oasys3" },
            { "oas", "application/vnd.fujitsu.oasys" },
            { "obd", "application/x-msbinder" },
            { "oda", "application/oda" },
            { "odb", "application/vnd.oasis.opendocument.database" },
            { "odc", "application/vnd.oasis.opendocument.chart" },
            { "odf", "application/vnd.oasis.opendocument.formula" },
            { "odft", "application/vnd.oasis.opendocument.formula-template" },
            { "odg", "application/vnd.oasis.opendocument.graphics" },
            { "odi", "application/vnd.oasis.opendocument.image" },
            { "odm", "application/vnd.oasis.opendocument.text-master" },
            { "odp", "application/vnd.oasis.opendocument.presentation" },
            { "ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { "odt", "application/vnd.oasis.opendocument.text" },
            { "oga", "audio/ogg" },
            { "ogg", "audio/ogg" },
            { "ogv", "video/ogg" },
            { "ogx", "application/ogg" },
            { "onepkg", "application/onenote" },
            { "onetmp", "application/onenote" },
            { "onetoc", "application/onenote" },
            { "onetoc2", "application/onenote" },
            { "opf", "application/oebps-package+xml" },
            { "oprc", "application/vnd.palm" },
            { "org", "application/vnd.lotus-organizer" },
            { "osf", "application/vnd.yamaha.openscoreformat" },
            { "osfpvg", "application/vnd.yamaha.openscoreformat.osfpvg+xml" },
            { "otc", "application/vnd.oasis.opendocument.chart-template" },
            { "otf", "application/x-font-otf" },
            { "otg", "application/vnd.oasis.opendocument.graphics-template" },
            { "oth", "application/vnd.oasis.opendocument.text-web" },
            { "oti", "application/vnd.oasis.opendocument.image-template" },
            { "otp", "application/vnd.oasis.opendocument.presentation-template" },
            { "ots", "application/vnd.oasis.opendocument.spreadsheet-template" },
            { "ott", "application/vnd.oasis.opendocument.text-template" },
            { "oxt", "application/vnd.openofficeorg.extension" },
            { "p", "text/x-pascal" },
            { "p10", "application/pkcs10" },
            { "p12", "application/x-pkcs12" },
            { "p7b", "application/x-pkcs7-certificates" },
            { "p7c", "application/pkcs7-mime" },
            { "p7m", "application/pkcs7-mime" },
            { "p7r", "application/x-pkcs7-certreqresp" },
            { "p7s", "application/pkcs7-signature" },
            { "p8", "application/pkcs8" },
            { "pas", "text/x-pascal" },
            { "paw", "application/vnd.pawaafile" },
            { "pbd", "application/vnd.powerbuilder6" },
            { "pbm", "image/x-portable-bitmap" },
            { "pcf", "application/x-font-pcf" },
            { "pcl", "application/vnd.hp-pcl" },
            { "pclxl", "application/vnd.hp-pclxl" },
            { "pct", "image/x-pict" },
            { "pcurl", "application/vnd.curl.pcurl" },
            { "pcx", "image/x-pcx" },
            { "pdb", "application/vnd.palm" },
            { "pdf", "application/pdf" },
            { "pfa", "application/x-font-type1" },
            { "pfb", "application/x-font-type1" },
            { "pfm", "application/x-font-type1" },
            { "pfr", "application/font-tdpfr" },
            { "pfx", "application/x-pkcs12" },
            { "pgm", "image/x-portable-graymap" },
            { "pgn", "application/x-chess-pgn" },
            { "pgp", "application/pgp-encrypted" },
            { "pic", "image/x-pict" },
            { "pkg", "application/octet-stream" },
            { "pki", "application/pkixcmp" },
            { "pkipath", "application/pkix-pkipath" },
            { "plb", "application/vnd.3gpp.pic-bw-large" },
            { "plc", "application/vnd.mobius.plc" },
            { "plf", "application/vnd.pocketlearn" },
            { "pls", "application/pls+xml" },
            { "pml", "application/vnd.ctc-posml" },
            { "png", "image/png" },
            { "pnm", "image/x-portable-anymap" },
            { "portpkg", "application/vnd.macports.portpkg" },
            { "pot", "application/vnd.ms-powerpoint" },
            { "potm", "application/vnd.ms-powerpoint.template.macroenabled.12" },
            { "potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
            { "ppam", "application/vnd.ms-powerpoint.addin.macroenabled.12" },
            { "ppd", "application/vnd.cups-ppd" },
            { "ppm", "image/x-portable-pixmap" },
            { "pps", "application/vnd.ms-powerpoint" },
            { "ppsm", "application/vnd.ms-powerpoint.slideshow.macroenabled.12" },
            { "ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
            { "ppt", "application/vnd.ms-powerpoint" },
            { "pptm", "application/vnd.ms-powerpoint.presentation.macroenabled.12" },
            { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { "pqa", "application/vnd.palm" },
            { "prc", "application/x-mobipocket-ebook" },
            { "pre", "application/vnd.lotus-freelance" },
            { "prf", "application/pics-rules" },
            { "ps", "application/postscript" },
            { "psb", "application/vnd.3gpp.pic-bw-small" },
            { "psd", "image/vnd.adobe.photoshop" },
            { "psf", "application/x-font-linux-psf" },
            { "pskcxml", "application/pskc+xml" },
            { "ptid", "application/vnd.pvi.ptid1" },
            { "pub", "application/x-mspublisher" },
            { "pvb", "application/vnd.3gpp.pic-bw-var" },
            { "pwn", "application/vnd.3m.post-it-notes" },
            { "pya", "audio/vnd.ms-playready.media.pya" },
            { "pyv", "video/vnd.ms-playready.media.pyv" },
            { "qam", "application/vnd.epson.quickanime" },
            { "qbo", "application/vnd.intu.qbo" },
            { "qfx", "application/vnd.intu.qfx" },
            { "qps", "application/vnd.publishare-delta-tree" },
            { "qt", "video/quicktime" },
            { "qwd", "application/vnd.quark.quarkxpress" },
            { "qwt", "application/vnd.quark.quarkxpress" },
            { "qxb", "application/vnd.quark.quarkxpress" },
            { "qxd", "application/vnd.quark.quarkxpress" },
            { "qxl", "application/vnd.quark.quarkxpress" },
            { "qxt", "application/vnd.quark.quarkxpress" },
            { "ra", "audio/x-pn-realaudio" },
            { "ram", "audio/x-pn-realaudio" },
            { "rar", "application/x-rar-compressed" },
            { "ras", "image/x-cmu-raster" },
            { "rcprofile", "application/vnd.ipunplugged.rcprofile" },
            { "rdf", "application/rdf+xml" },
            { "rdz", "application/vnd.data-vision.rdz" },
            { "rep", "application/vnd.businessobjects" },
            { "res", "application/x-dtbresource+xml" },
            { "rgb", "image/x-rgb" },
            { "rif", "application/reginfo+xml" },
            { "rip", "audio/vnd.rip" },
            { "rl", "application/resource-lists+xml" },
            { "rlc", "image/vnd.fujixerox.edmics-rlc" },
            { "rld", "application/resource-lists-diff+xml" },
            { "rm", "application/vnd.rn-realmedia" },
            { "rmi", "audio/midi" },
            { "rmp", "audio/x-pn-realaudio-plugin" },
            { "rms", "application/vnd.jcp.javame.midlet-rms" },
            { "rnc", "application/relax-ng-compact-syntax" },
            { "roff", "text/troff" },
            { "rp9", "application/vnd.cloanto.rp9" },
            { "rpss", "application/vnd.nokia.radio-presets" },
            { "rpst", "application/vnd.nokia.radio-preset" },
            { "rq", "application/sparql-query" },
            { "rs", "application/rls-services+xml" },
            { "rsd", "application/rsd+xml" },
            { "rss", "application/rss+xml" },
            { "rtf", "application/rtf" },
            { "rtx", "text/richtext" },
            { "s", "text/x-asm" },
            { "saf", "application/vnd.yamaha.smaf-audio" },
            { "sbml", "application/sbml+xml" },
            { "sc", "application/vnd.ibm.secure-container" },
            { "scd", "application/x-msschedule" },
            { "scm", "application/vnd.lotus-screencam" },
            { "scq", "application/scvp-cv-request" },
            { "scs", "application/scvp-cv-response" },
            { "scurl", "text/vnd.curl.scurl" },
            { "sda", "application/vnd.stardivision.draw" },
            { "sdc", "application/vnd.stardivision.calc" },
            { "sdd", "application/vnd.stardivision.impress" },
            { "sdkd", "application/vnd.solent.sdkm+xml" },
            { "sdkm", "application/vnd.solent.sdkm+xml" },
            { "sdp", "application/sdp" },
            { "sdw", "application/vnd.stardivision.writer" },
            { "see", "application/vnd.seemail" },
            { "seed", "application/vnd.fdsn.seed" },
            { "sema", "application/vnd.sema" },
            { "semd", "application/vnd.semd" },
            { "semf", "application/vnd.semf" },
            { "ser", "application/java-serialized-object" },
            { "setpay", "application/set-payment-initiation" },
            { "setreg", "application/set-registration-initiation" },
            { "sfd-hdstx", "application/vnd.hydrostatix.sof-data" },
            { "sfs", "application/vnd.spotfire.sfs" },
            { "sgl", "application/vnd.stardivision.writer-global" },
            { "sgm", "text/sgml" },
            { "sgml", "text/sgml" },
            { "sh", "application/x-sh" },
            { "shar", "application/x-shar" },
            { "shf", "application/shf+xml" },
            { "sig", "application/pgp-signature" },
            { "silo", "model/mesh" },
            { "sis", "application/vnd.symbian.install" },
            { "sisx", "application/vnd.symbian.install" },
            { "sit", "application/x-stuffit" },
            { "sitx", "application/x-stuffitx" },
            { "skd", "application/vnd.koan" },
            { "skm", "application/vnd.koan" },
            { "skp", "application/vnd.koan" },
            { "skt", "application/vnd.koan" },
            { "sldm", "application/vnd.ms-powerpoint.slide.macroenabled.12" },
            { "sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide" },
            { "slt", "application/vnd.epson.salt" },
            { "sm", "application/vnd.stepmania.stepchart" },
            { "smf", "application/vnd.stardivision.math" },
            { "smi", "application/smil+xml" },
            { "smil", "application/smil+xml" },
            { "snd", "audio/basic" },
            { "snf", "application/x-font-snf" },
            { "so", "application/octet-stream" },
            { "spc", "application/x-pkcs7-certificates" },
            { "spf", "application/vnd.yamaha.smaf-phrase" },
            { "spl", "application/x-futuresplash" },
            { "spot", "text/vnd.in3d.spot" },
            { "spp", "application/scvp-vp-response" },
            { "spq", "application/scvp-vp-request" },
            { "spx", "audio/ogg" },
            { "src", "application/x-wais-source" },
            { "sru", "application/sru+xml" },
            { "srx", "application/sparql-results+xml" },
            { "sse", "application/vnd.kodak-descriptor" },
            { "ssf", "application/vnd.epson.ssf" },
            { "ssml", "application/ssml+xml" },
            { "st", "application/vnd.sailingtracker.track" },
            { "stc", "application/vnd.sun.xml.calc.template" },
            { "std", "application/vnd.sun.xml.draw.template" },
            { "stf", "application/vnd.wt.stf" },
            { "sti", "application/vnd.sun.xml.impress.template" },
            { "stk", "application/hyperstudio" },
            { "stl", "application/vnd.ms-pki.stl" },
            { "str", "application/vnd.pg.format" },
            { "stw", "application/vnd.sun.xml.writer.template" },
            { "sub", "image/vnd.dvb.subtitle" },
            { "sus", "application/vnd.sus-calendar" },
            { "susp", "application/vnd.sus-calendar" },
            { "sv4cpio", "application/x-sv4cpio" },
            { "sv4crc", "application/x-sv4crc" },
            { "svc", "application/vnd.dvb.service" },
            { "svd", "application/vnd.svd" },
            { "svg", "image/svg+xml" },
            { "svgz", "image/svg+xml" },
            { "swa", "application/x-director" },
            { "swf", "application/x-shockwave-flash" },
            { "swi", "application/vnd.aristanetworks.swi" },
            { "sxc", "application/vnd.sun.xml.calc" },
            { "sxd", "application/vnd.sun.xml.draw" },
            { "sxg", "application/vnd.sun.xml.writer.global" },
            { "sxi", "application/vnd.sun.xml.impress" },
            { "sxm", "application/vnd.sun.xml.math" },
            { "sxw", "application/vnd.sun.xml.writer" },
            { "t", "text/troff" },
            { "tao", "application/vnd.tao.intent-module-archive" },
            { "tar", "application/x-tar" },
            { "tcap", "application/vnd.3gpp2.tcap" },
            { "tcl", "application/x-tcl" },
            { "teacher", "application/vnd.smart.teacher" },
            { "tei", "application/tei+xml" },
            { "teicorpus", "application/tei+xml" },
            { "tex", "application/x-tex" },
            { "texi", "application/x-texinfo" },
            { "texinfo", "application/x-texinfo" },
            { "text", "text/plain" },
            { "tfi", "application/thraud+xml" },
            { "tfm", "application/x-tex-tfm" },
            { "thmx", "application/vnd.ms-officetheme" },
            { "tif", "image/tiff" },
            { "tiff", "image/tiff" },
            { "tmo", "application/vnd.tmobile-livetv" },
            { "torrent", "application/x-bittorrent" },
            { "tpl", "application/vnd.groove-tool-template" },
            { "tpt", "application/vnd.trid.tpt" },
            { "tr", "text/troff" },
            { "tra", "application/vnd.trueapp" },
            { "trm", "application/x-msterminal" },
            { "tsd", "application/timestamped-data" },
            { "tsv", "text/tab-separated-values" },
            { "ttc", "application/x-font-ttf" },
            { "ttf", "application/x-font-ttf" },
            { "ttl", "text/turtle" },
            { "twd", "application/vnd.simtech-mindmapper" },
            { "twds", "application/vnd.simtech-mindmapper" },
            { "txd", "application/vnd.genomatix.tuxedo" },
            { "txf", "application/vnd.mobius.txf" },
            { "txt", "text/plain" },
            { "u32", "application/x-authorware-bin" },
            { "udeb", "application/x-debian-package" },
            { "ufd", "application/vnd.ufdl" },
            { "ufdl", "application/vnd.ufdl" },
            { "umj", "application/vnd.umajin" },
            { "unityweb", "application/vnd.unity" },
            { "uoml", "application/vnd.uoml+xml" },
            { "uri", "text/uri-list" },
            { "uris", "text/uri-list" },
            { "urls", "text/uri-list" },
            { "ustar", "application/x-ustar" },
            { "utz", "application/vnd.uiq.theme" },
            { "uu", "text/x-uuencode" },
            { "uva", "audio/vnd.dece.audio" },
            { "uvd", "application/vnd.dece.data" },
            { "uvf", "application/vnd.dece.data" },
            { "uvg", "image/vnd.dece.graphic" },
            { "uvh", "video/vnd.dece.hd" },
            { "uvi", "image/vnd.dece.graphic" },
            { "uvm", "video/vnd.dece.mobile" },
            { "uvp", "video/vnd.dece.pd" },
            { "uvs", "video/vnd.dece.sd" },
            { "uvt", "application/vnd.dece.ttml+xml" },
            { "uvu", "video/vnd.uvvu.mp4" },
            { "uvv", "video/vnd.dece.video" },
            { "uvva", "audio/vnd.dece.audio" },
            { "uvvd", "application/vnd.dece.data" },
            { "uvvf", "application/vnd.dece.data" },
            { "uvvg", "image/vnd.dece.graphic" },
            { "uvvh", "video/vnd.dece.hd" },
            { "uvvi", "image/vnd.dece.graphic" },
            { "uvvm", "video/vnd.dece.mobile" },
            { "uvvp", "video/vnd.dece.pd" },
            { "uvvs", "video/vnd.dece.sd" },
            { "uvvt", "application/vnd.dece.ttml+xml" },
            { "uvvu", "video/vnd.uvvu.mp4" },
            { "uvvv", "video/vnd.dece.video" },
            { "uvvx", "application/vnd.dece.unspecified" },
            { "uvx", "application/vnd.dece.unspecified" },
            { "vcd", "application/x-cdlink" },
            { "vcf", "text/x-vcard" },
            { "vcg", "application/vnd.groove-vcard" },
            { "vcs", "text/x-vcalendar" },
            { "vcx", "application/vnd.vcx" },
            { "vis", "application/vnd.visionary" },
            { "viv", "video/vnd.vivo" },
            { "vor", "application/vnd.stardivision.writer" },
            { "vox", "application/x-authorware-bin" },
            { "vrml", "model/vrml" },
            { "vsd", "application/vnd.visio" },
            { "vsf", "application/vnd.vsf" },
            { "vss", "application/vnd.visio" },
            { "vst", "application/vnd.visio" },
            { "vsw", "application/vnd.visio" },
            { "vtu", "model/vnd.vtu" },
            { "vxml", "application/voicexml+xml" },
            { "w3d", "application/x-director" },
            { "wad", "application/x-doom" },
            { "wav", "audio/x-wav" },
            { "wax", "audio/x-ms-wax" },
            { "wbmp", "image/vnd.wap.wbmp" },
            { "wbs", "application/vnd.criticaltools.wbs+xml" },
            { "wbxml", "application/vnd.wap.wbxml" },
            { "wcm", "application/vnd.ms-works" },
            { "wdb", "application/vnd.ms-works" },
            { "weba", "audio/webm" },
            { "webm", "video/webm" },
            { "webp", "image/webp" },
            { "wg", "application/vnd.pmi.widget" },
            { "wgt", "application/widget" },
            { "wks", "application/vnd.ms-works" },
            { "wm", "video/x-ms-wm" },
            { "wma", "audio/x-ms-wma" },
            { "wmd", "application/x-ms-wmd" },
            { "wmf", "application/x-msmetafile" },
            { "wml", "text/vnd.wap.wml" },
            { "wmlc", "application/vnd.wap.wmlc" },
            { "wmls", "text/vnd.wap.wmlscript" },
            { "wmlsc", "application/vnd.wap.wmlscriptc" },
            { "wmv", "video/x-ms-wmv" },
            { "wmx", "video/x-ms-wmx" },
            { "wmz", "application/x-ms-wmz" },
            { "woff", "application/x-font-woff" },
            { "wpd", "application/vnd.wordperfect" },
            { "wpl", "application/vnd.ms-wpl" },
            { "wps", "application/vnd.ms-works" },
            { "wqd", "application/vnd.wqd" },
            { "wri", "application/x-mswrite" },
            { "wrl", "model/vrml" },
            { "wsdl", "application/wsdl+xml" },
            { "wspolicy", "application/wspolicy+xml" },
            { "wtb", "application/vnd.webturbo" },
            { "wvx", "video/x-ms-wvx" },
            { "x32", "application/x-authorware-bin" },
            { "x3d", "application/vnd.hzn-3d-crossword" },
            { "xap", "application/x-silverlight-app" },
            { "xar", "application/vnd.xara" },
            { "xbap", "application/x-ms-xbap" },
            { "xbd", "application/vnd.fujixerox.docuworks.binder" },
            { "xbm", "image/x-xbitmap" },
            { "xdf", "application/xcap-diff+xml" },
            { "xdm", "application/vnd.syncml.dm+xml" },
            { "xdp", "application/vnd.adobe.xdp+xml" },
            { "xdssc", "application/dssc+xml" },
            { "xdw", "application/vnd.fujixerox.docuworks" },
            { "xenc", "application/xenc+xml" },
            { "xer", "application/patch-ops-error+xml" },
            { "xfdf", "application/vnd.adobe.xfdf" },
            { "xfdl", "application/vnd.xfdl" },
            { "xht", "application/xhtml+xml" },
            { "xhtml", "application/xhtml+xml" },
            { "xhvml", "application/xv+xml" },
            { "xif", "image/vnd.xiff" },
            { "xla", "application/vnd.ms-excel" },
            { "xlam", "application/vnd.ms-excel.addin.macroenabled.12" },
            { "xlc", "application/vnd.ms-excel" },
            { "xlm", "application/vnd.ms-excel" },
            { "xls", "application/vnd.ms-excel" },
            { "xlsb", "application/vnd.ms-excel.sheet.binary.macroenabled.12" },
            { "xlsm", "application/vnd.ms-excel.sheet.macroenabled.12" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "xlt", "application/vnd.ms-excel" },
            { "xltm", "application/vnd.ms-excel.template.macroenabled.12" },
            { "xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
            { "xlw", "application/vnd.ms-excel" },
            { "xml", "application/xml" },
            { "xo", "application/vnd.olpc-sugar" },
            { "xop", "application/xop+xml" },
            { "xpi", "application/x-xpinstall" },
            { "xpm", "image/x-xpixmap" },
            { "xpr", "application/vnd.is-xpr" },
            { "xps", "application/vnd.ms-xpsdocument" },
            { "xpw", "application/vnd.intercon.formnet" },
            { "xpx", "application/vnd.intercon.formnet" },
            { "xsl", "application/xml" },
            { "xslt", "application/xslt+xml" },
            { "xsm", "application/vnd.syncml+xml" },
            { "xspf", "application/xspf+xml" },
            { "xul", "application/vnd.mozilla.xul+xml" },
            { "xvm", "application/xv+xml" },
            { "xvml", "application/xv+xml" },
            { "xwd", "image/x-xwindowdump" },
            { "xyz", "chemical/x-xyz" },
            { "yang", "application/yang" },
            { "yin", "application/yin+xml" },
            { "zaz", "application/vnd.zzazz.deck+xml" },
            { "zip", "application/zip" },
            { "zir", "application/vnd.zul" },
            { "zirz", "application/vnd.zul" },
            { "zmm", "application/vnd.handheld-entertainment+xml" },
        };
                #endregion
                public StreamPlayerOptions(Stream Content, string Extension, bool Loop = false)
                {
                    this.Content = Content; MimeType = MimeTypes[Extension.ToLower().Trim().TrimStart('.')]; this.Loop = Loop;
                }
                public Stream Content = null;
                public float Volume = 1;
                public bool Loop = false;
                /// <summary>
                /// Stream type.
                /// </summary>
                public StreamType Type = StreamType.Music;
                int? _SampleRate = null;
                /// <summary>
                /// Frequency in Hertz (Hz). 
                /// </summary>
                // Always returns 0 (Unspecified). OPTIONAL. Do not enter unless you are sure about the value.
                public int SampleRate
                {
                    get
                    {
                        if (_SampleRate.HasValue) return _SampleRate.Value;
                        if (!Content.CanSeek) return 11025;
                        Content.Seek(24, SeekOrigin.Begin);
                        byte[] val = new byte[4];
                        Content.Read(val, 0, 4);
                        return System.BitConverter.ToInt32(val, 0);
                    }
                    //set { _SampleRate = value; }
                }
                /// <summary>
                /// Number of channels.
                /// </summary>
                public int Channels
                {
                    get
                    {
                        if (!Content.CanSeek) return 1;
                        Content.Seek(22, SeekOrigin.Begin);
                        byte[] val = new byte[2];
                        Content.Read(val, 0, 2);
                        return System.BitConverter.ToInt16(val, 0);
                    }
                }
                /// <summary>
                /// Mono or stereo.
                /// </summary>
                public ChannelOut Config
                {
                    get
                    {
                        if (!Content.CanSeek) return ChannelOut.Mono;
                        Content.Seek(22, SeekOrigin.Begin);
                        byte[] val = new byte[2];
                        Content.Read(val, 0, 2);
                        switch (System.BitConverter.ToInt16(val, 0))
                        {
                            case 1: return ChannelOut.Mono;
                            case 2: return ChannelOut.Stereo;
                            case 3: return ChannelOut.Stereo | ChannelOut.FrontCenter;
                            case 4: return ChannelOut.Quad;
                            case 5: return ChannelOut.Quad | ChannelOut.FrontCenter;
                            case 6: return ChannelOut.FivePointOne;
                            case 7: return ChannelOut.FivePointOne | ChannelOut.BackCenter;
                            case 8: return ChannelOut.SevenPointOne;
                        }
                        return ChannelOut.None;
                    }
                }
                /// <summary>
                /// Audio encoding.
                /// </summary>
                public Encoding Format
                {
                    get
                    {
                        if (!Content.CanSeek) return Encoding.Default;
                        Content.Seek(34, SeekOrigin.Begin);
                        byte[] val = new byte[2];
                        Content.Read(val, 0, 2);
                        switch (System.BitConverter.ToInt16(val, 0))
                        {
                            case 8: return Encoding.Pcm8bit;
                            case 16: return Encoding.Pcm16bit;
                            case 32: return Encoding.PcmFloat;
                        }
                        return Encoding.Default;
                    }
                }
                /// <summary>
                /// Bits per sample.
                /// </summary>
                public int BitsPerSample
                {
                    get
                    {
                        if (!Content.CanSeek) return 16;
                        Content.Seek(34, SeekOrigin.Begin);
                        byte[] val = new byte[2];
                        Content.Read(val, 0, 2);
                        return System.BitConverter.ToInt16(val, 0);
                    }
                }
                /// <summary>
                /// Number of samples.
                /// </summary>
                public int Samples
                {
                    get
                    {
                        if (!Content.CanSeek) return 0;
                        Content.Seek(40, SeekOrigin.Begin);
                        byte[] val = new byte[4];
                        Content.Read(val, 0, 4);
                        return System.BitConverter.ToInt32(val, 0) * 8 / Channels / BitsPerSample;
                    }
                }
                /// <summary>
                /// Length of the audio clip.
                /// </summary>.
                public int SizeInBytes { get { return (int)Content.Length; } }
                /// <summary>
                /// Mode. Stream or static.
                /// </summary>
                public AudioTrackMode Mode = AudioTrackMode.Stream;
                /// <summary>
                /// Mime type. Default is audio/x-wav.
                /// </summary>
                public string MimeType { get; } = "audio/x-wav";

                public enum AudioTrackMode
                {
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Creation mode where audio data is transferred from Java to the native layer
                    ///  only once before the audio starts playing.
                    /// </para>
                    /// </summary>
                    Static,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Creation mode where audio data is streamed from Java to the native layer
                    ///  as the audio is playing.
                    /// </para>
                    /// </summary>
                    Stream
                }


                public System.TimeSpan Duration
                {
                    get
                    {
                        Content.Seek(40, SeekOrigin.Begin);
                        byte[] val = new byte[4];
                        Content.Read(val, 0, 4);
                        double Size = System.BitConverter.ToUInt32(val, 0);
                        Content.Seek(28, SeekOrigin.Begin);
                        val = new byte[4];
                        Content.Read(val, 0, 4);
                        double ByteRate = System.BitConverter.ToUInt32(val, 0);
                        return System.TimeSpan.FromSeconds(Size / ByteRate);
                    }
                }
                public enum ChannelOut
                {
                    /// <summary>To be added.</summary>
                    None,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    FivePointOne = 252,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    SevenPointOne = 1020,
                    C7point1Surround = 6396,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    BackCenter = 1024,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    BackLeft = 64,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    BackRight = 128,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Default audio channel mask </para>
                    /// </summary>
                    Default = 1,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    FrontCenter = 16,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    FrontLeft = 4,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    FrontLeftOfCenter = 256,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    FrontRight = 8,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    FrontRightOfCenter = 512,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    LowFrequency = 32,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    Mono = 4,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    Quad = 204,
                    SideLeft = 2048,
                    SideRight = 4096,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    Stereo = 12,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc" />
                    /// </summary>
                    Surround = 1052
                }
                public enum Encoding
                {
                    Ac3 = 5,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Default audio data format </para>
                    /// </summary>
                    Default = 1,
                    Dts = 7,
                    DtsHd,
                    EAc3 = 6,
                    Iec61937 = 13,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Invalid audio data format </para>
                    /// </summary>
                    Invalid = 0,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Audio data format: PCM 16 bit per sample. Guaranteed to be supported by devices. </para>
                    /// </summary>
                    Pcm16bit = 2,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Audio data format: PCM 8 bit per sample. Not guaranteed to be supported by devices. </para>
                    /// </summary>
                    Pcm8bit,
                    PcmFloat
                }
                public enum StreamType
                {
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">Use this constant as the value for audioStreamType to request that
                    ///  the default stream type for notifications be used.  Currently the
                    ///  default stream type is <c><see cref="F:Android.Media.Stream.Notification" /></c>.
                    /// </para>
                    /// </summary>
                    NotificationDefault = -1,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for alarms </para>
                    /// </summary>
                    Alarm = 4,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for DTMF Tones </para>
                    /// </summary>
                    Dtmf = 8,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for music playback </para>
                    /// </summary>
                    Music = 3,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for notifications </para>
                    /// </summary>
                    Notification = 5,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for the phone ring </para>
                    /// </summary>
                    Ring = 2,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for system sounds </para>
                    /// </summary>
                    System = 1,
                    /// <summary>
                    ///     <para tool="javadoc-to-mdoc">The audio stream for phone calls </para>
                    /// </summary>
                    VoiceCall = 0
                }
            }
            public enum Sounds : byte
            {
                Violin_G,
                Violin_D,
                Violin_A,
                Violin_E,
                Cello_C,
                Cello_G,
                Cello_D,
                Cello_A
            }
            public static StreamPlayer Create(Sounds Sound, bool Loop = false, float Volume = 1)
            {
                string Name = "";
                switch (Sound)
                {
                    case Sounds.Violin_G:
                        Name = "ViolinG.wav";
                        break;
                    case Sounds.Violin_D:
                        Name = "ViolinD.wav";
                        break;
                    case Sounds.Violin_A:
                        Name = "ViolinA.wav";
                        break;
                    case Sounds.Violin_E:
                        Name = "ViolinE.wav";
                        break;
                    case Sounds.Cello_C:
                        Name = "CelloCC.wav";
                        break;
                    case Sounds.Cello_G:
                        Name = "CelloGG.wav";
                        break;
                    case Sounds.Cello_D:
                        Name = "CelloD.wav";
                        break;
                    case Sounds.Cello_A:
                        Name = "CelloA.wav";
                        break;
                    default:
                        break;
                }
                return Create(new StreamPlayerOptions(Resources.GetStream("Sounds." + Name), Path.GetExtension(Name))
                { Volume = Volume, Loop = Loop });
            }
            public static StreamPlayer Play(Sounds Sound, StreamPlayerOptions Options)
            {
                var Return = Create(Sound, Options);
                Return.Play();
                return Return;
            }
            public static StreamPlayer Create(Sounds Sound, StreamPlayerOptions Options)
            {
                string Name = "";
                switch (Sound)
                {
                    case Sounds.Violin_G:
                        Name = "ViolinG.wav";
                        break;
                    case Sounds.Violin_D:
                        Name = "ViolinD.wav";
                        break;
                    case Sounds.Violin_A:
                        Name = "ViolinA.wav";
                        break;
                    case Sounds.Violin_E:
                        Name = "ViolinE.wav";
                        break;
                    case Sounds.Cello_C:
                        Name = "CelloCC.wav";
                        break;
                    case Sounds.Cello_G:
                        Name = "CelloGG.wav";
                        break;
                    case Sounds.Cello_D:
                        Name = "CelloD.wav";
                        break;
                    case Sounds.Cello_A:
                        Name = "CelloA.wav";
                        break;
                    default:
                        break;
                }
                Options.Content = Resources.GetStream("Sounds." + Name);
                return Create(Options);
            }
            public static StreamPlayer Play(Sounds Sound, bool Loop = false, float Volume = 1)
            {
                var Return = Create(Sound, Loop, Volume);
                Return.Play();
                return Return;
            }
#if __IOS__
            public static StreamPlayer Create(StreamPlayerOptions Options)
            {
                var Return = new StreamPlayer();
                Return.Init(Options);
                return Return;
            }
            protected void Init(StreamPlayerOptions Options)
            {
                _player = AVAudioPlayer.FromData(NSData.FromStream(Options.Content));
                _player.NumberOfLoops = Options.Loop ? 0 : -1;
                _player.Volume = Options.Volume;
            }
            AVAudioPlayer _player;
            [System.Obsolete("Only used in 0.10.0a105. Use Create(StreamPlayerOptions).")]
            public static StreamPlayer Create(Stream Content, bool Loop = false, float Volume = 1)
            {
                var Return = new StreamPlayer();
                Return.Init(Content, Loop, Volume);
                return Return;
            }
            [System.Obsolete("Only used in 0.10.0a105. Use Init(StreamPlayerOptions).")]
            protected void Init(Stream Content, bool Loop, float Volume)
            {
                _player = AVAudioPlayer.FromData(NSData.FromStream(Content));
                _player.NumberOfLoops = Loop ? 0 : -1;
                _player.Volume = Volume;
                //_player.FinishedPlaying += (object sender, AVStatusEventArgs e) => { _player = null; };
            }
            public void Play()
            { _player.Play(); }
            public void Pause()
            { _player.Pause(); }
            public void Stop()
            { _player.Stop(); }
            public event System.EventHandler Complete
            {
                add { _player.FinishedPlaying += (System.EventHandler<AVStatusEventArgs>)(System.MulticastDelegate)value; }
                remove { _player.FinishedPlaying -= (System.EventHandler<AVStatusEventArgs>)(System.MulticastDelegate)value; }
            }
            public float Volume { get { return _player.Volume; } set { _player.Volume = value; } }
            public bool Loop { get { return _player.NumberOfLoops == -1; } set { _player.NumberOfLoops = value ? -1 : 0; } }
            ~StreamPlayer()
            { _player.Dispose(); }
            #elif __ANDROID__
            AudioTrack _player;
            public bool _prepared { get; private set; }
            bool _loop;
            float _volume;
            int _frames;
            public static StreamPlayer Create(StreamPlayerOptions Options)
            {
                var Return = new StreamPlayer();
                Return.Init(Options);
                return Return;
            }
            protected void Init(StreamPlayerOptions Options)
            {
                int SizeInBytes = Options.SizeInBytes - 44;
                _player = new AudioTrack(
                // Stream type
                (Android.Media.Stream)Options.Type,
                // Frequency
                Options.SampleRate,
                // Mono or stereo
                (ChannelOut)Options.Config,
                // Audio encoding
                (Encoding)Options.Format,
                // Length of the audio clip.
                SizeInBytes,
                // Mode. Stream or static.
                AudioTrackMode.Static);
                _loop = Options.Loop;
                _volume = Options.Volume;
                _frames = Options.Samples / Options.Channels;
                _player.SetVolume(_volume = Options.Volume);
                _player.Write(Options.Content.ReadFully(true), 0, (int)Options.Content.Length);
                _prepared = true;
            }
            public void Play()
            {
                if (!_prepared) return;
                _player.ReloadStaticData();
                if (_loop) _player.SetLoopPoints(0, _frames, -1);
                _player.Play();
            }
            public void Pause()
            { if (_prepared) _player.Pause(); }
            public void Stop()
            {
                if (_player == null)
                    return;

                if (_loop) _player.SetLoopPoints(0, 0, 0);

                _player.Stop();
            }
            public event EventHandler Complete
            {
                add
                {
                    _player.MarkerReached += MarkerReachedEventHandler(value);
                }
                remove
                {
                    _player.MarkerReached -= MarkerReachedEventHandler(value);
                }
            }
            protected EventHandler<AudioTrack.MarkerReachedEventArgs>
                MarkerReachedEventHandler(EventHandler value)
            {
                return (object sender, AudioTrack.MarkerReachedEventArgs e) =>
                   {
                       value(sender, e);
                   };
            }
            public float Volume { get { return _volume; } set { _player?.SetVolume(_volume = value); } }
            public bool Loop { get { return _loop; } set { _loop = value; if (_prepared && !value) _player.SetLoopPoints(0, 0, 0); } }
            ~StreamPlayer()
            {   Stop();
                _player.Dispose();
                _player = null;
                _prepared = false;
            }
#elif __ANDROID__ && RESAMPLE
            AudioTrack _player;
            Stream _content;
            public bool _prepared { get; private set; }
            bool _loop;
            float _volume;
            int Rate, SampleRate;
            public static StreamPlayer Create(StreamPlayerOptions Options)
            {
                var Return = new StreamPlayer();
                Return.Init(Options);
                return Return;
            }
            protected void Init(StreamPlayerOptions Options)
            {// To get preferred buffer size and sampling rate.
                AudioManager audioManager = (AudioManager)
                    Forms.Context.GetSystemService(Android.Content.Context.AudioService);
                Rate = int.Parse(audioManager.GetProperty(AudioManager.PropertyOutputSampleRate));
                //string Size = audioManager.GetProperty(AudioManager.PropertyOutputFramesPerBuffer);
                SampleRate = Options.SampleRate;

                _content = Options.Content;
                int SizeInBytes = Options.SizeInBytes - 44;
                _player = new AudioTrack(
                // Stream type
                (Android.Media.Stream)Options.Type,
                // Frequency
                Rate,
                // Mono or stereo
                (ChannelOut)Options.Config,
                // Audio encoding
                (Encoding)Options.Format,
                // Length of the audio clip.
                SizeInBytes,
                // Mode. Stream or static.
                (AudioTrackMode)Options.Mode);
                _loop = Options.Loop;
                _volume = Options.Volume;
                _player.SetVolume(_volume = Options.Volume);
#if true
                //int ch = Options.Channels;
                //_start = 0;// (int)Options.Content.Length / ch;
                //_stop = (int)Options.Content.Length;// / ch / 2 / 2 + 16000
#elif false
                _player.SetNotificationMarkerPosition(SizeInBytes / 2);
                _player.MarkerReached += (object sender, AudioTrack.MarkerReachedEventArgs e) =>
                       { if (_loop) e.Track.SetPlaybackHeadPosition(0); };
#elif false
                Device.StartTimer(Options.Duration, () => { if (_loop) _player.SetPlaybackHeadPosition(0); return _loop; });
#endif
                _prepared = true;
            }
            [Obsolete("Only used in 0.10.0a105. Use Create(StreamPlayerOptions).")]
            public static StreamPlayer Create(Stream Content, bool Loop = false, float Volume = 1)
            {
                var Return = new StreamPlayer();
                Return.Init(Content, Loop, Volume);
                return Return;
            }
            [Obsolete("Only used in 0.10.0a105. Use Init(StreamPlayerOptions).")]
            protected void Init(Stream Content, bool Loop, float Volume)
            {
                _content = Content;
                _player = new AudioTrack(
                // Stream type
                Android.Media.Stream.Music,
                // Frequency
                11025,
                // Mono or stereo
                ChannelOut.Mono,
                // Audio encoding
                Encoding.Pcm16bit,
                // Length of the audio clip.
                (int)Content.Length,
                // Mode. Stream or static.
                AudioTrackMode.Stream);
                _loop = Loop;
                _volume = Volume;
                _player.SetVolume(_volume = Volume);
                _player.SetNotificationMarkerPosition((int)Content.Length / 2);
                _prepared = true;
            }
            Iterator<byte> Resampled;
            const int ShortBuffer = 256;
            bool _pause, _stop;
            Task Ongoing;
            void PlayTask()
            {
                if(!_pause)Resampled = new Iterator<byte>(
                    Resample(_content.ReadFully(true).Skip(44).ToArray(), 44, SampleRate, Rate));
                _stop = _pause = false;
                int x;
                do
                {
                    do
                    {
                        x = _player.PlaybackHeadPosition;
                        _player.Write(Resampled.Take(ShortBuffer), 0, ShortBuffer);
                        do
                        {   // Montior playback to find when done
                        } while (_player.PlaybackHeadPosition < x + ShortBuffer);
                    } while (Resampled.HasPeek && !_pause && !_stop);
                } while (_loop && !_pause && !_stop);
            }
            public void Play()
            {
                if (!_prepared) return;
                _player.Play();
                Ongoing = Task.Run(new Action(PlayTask));
            }
            public void Pause()
            { if (_prepared) _pause = true; }
            public void Stop()
            {
                if (_player == null)
                    return;
                _stop = true;
                Do(Ongoing);

                _player.Stop();
                _player.Dispose();
                _player = null;
                _prepared = false;
            }
            public event EventHandler Complete
            {
                add
                {
                    _player.MarkerReached += MarkerReachedEventHandler(value);
                }
                remove
                {
                    _player.MarkerReached -= MarkerReachedEventHandler(value);
                }
            }
            protected EventHandler<AudioTrack.MarkerReachedEventArgs>
                MarkerReachedEventHandler(EventHandler value)
            {
                return (object sender, AudioTrack.MarkerReachedEventArgs e) =>
                   {
                       value(sender, e);
                   };
            }
            public float Volume { get { return _volume; } set { _player.SetVolume(_volume = value); } }
            public bool Loop { get { return _loop; } set { _loop = value; } }
            ~StreamPlayer()
            { Stop(); }
            public static IEnumerable<byte> Resample(byte[] samples, int fromSampleRate, int toSampleRate, int quality = 10)
            {
                int srcLength = samples.Length;
                var destLength = (long)samples.Length * toSampleRate / fromSampleRate;
                var dx = srcLength / destLength;

                // fmax : nyqist half of destination sampleRate
                // fmax / fsr = 0.5;
                var fmaxDivSR = 0.5;
                var r_g = 2 * fmaxDivSR;

                // Quality is half the window width
                var wndWidth2 = quality;
                var wndWidth = quality * 2;

                var x = 0;
                int i, j;
                double r_y;
                int tau;
                double r_w;
                double r_a;
                double r_snc;
                for (i = 0; i < destLength; ++i)
                {
                    r_y = 0.0;
                    for (tau = -wndWidth2; tau < wndWidth2; ++tau)
                    {
                        // input sample index
                        j = x + tau;

                        // Hann Window. Scale and calculate sinc
                        r_w = 0.5 - 0.5 * Math.Cos(2 * Math.Pi * (0.5 + (j - x) / wndWidth));
                        r_a = 2 * Math.Pi * (j - x) * fmaxDivSR;
                        r_snc = 1.0;
                        if (r_a != 0)
                            r_snc = Math.Sin(r_a) / r_a;

                        if ((j >= 0) && (j < srcLength))
                        {
                            r_y += r_g * r_w * r_snc * samples[j];
                        }
                    }
                    yield return (byte)r_y;
                    x += (int)dx;
                }
            }
            public class Iterator<T> : IEnumerator<T>//, IEnumerable<T>
            {
                private IEnumerator<T> _enumerator;
                private T _peek;
                private bool _didPeek;

                public Iterator(IEnumerable<T> enumerable) : this(enumerable.GetEnumerator()) { }

                public Iterator(IEnumerator<T> enumerator)
                {
                    if (enumerator == null)
                        throw new ArgumentNullException("enumerator");
                    _enumerator = enumerator;
                }

            #region IEnumerator implementation
                public bool MoveNext()
                {
                    return _didPeek ? !(_didPeek = false) : _enumerator.MoveNext();
                }

                public void Reset()
                {
                    _enumerator.Reset();
                    _didPeek = false;
                }

                object IEnumerator.Current { get { return this.Current; } }
            #endregion

            #region IDisposable implementation
                public void Dispose()
                {
                    _enumerator.Dispose();
                }
            #endregion

            #region IEnumerator<T> implementation
                public T Current
                {
                    get { return _didPeek ? _peek : _enumerator.Current; }
                }
            #endregion
                /*
            #region IEnumerable implementation
                public IEnumerator<T> GetEnumerator()
                {
                    return this; 
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this; 
                }
            #endregion
                */
                private void TryFetchPeek()
                {
                    if (!_didPeek && (_didPeek = _enumerator.MoveNext()))
                    {
                        _peek = _enumerator.Current;
                    }
                }

                public T Peek
                {
                    get
                    {
                        TryFetchPeek();
                        if (!_didPeek)
                            throw new InvalidOperationException("Enumeration already finished.");

                        return _peek;
                    }
                }

                public bool HasPeek
                { get { try { DoNothing(Peek); return true; } catch (InvalidOperationException) { return false; } } }

                public T[] Take(int Count)
                {
                    var Return = new T[Count];
                    for (int i = 0; i < Count && HasPeek; i++)
                    { MoveNext(); Return[i] = Current; }
                    return Return;
                }

                public void Skip(int Count)
                {
                    for (int i = 0; i < Count && HasPeek; i++)
                        MoveNext();
                }
            }
#elif NETFX_CORE
            MediaElement _player;
            public static StreamPlayer Create(StreamPlayerOptions Options)
            {
                var Return = new StreamPlayer();
                Return.Init(Options);
                return Return;
            }
            protected void Init(StreamPlayerOptions Options)
            {
                _player = new MediaElement
                {
                    IsMuted = false,
                    Position = new TimeSpan(0, 0, 0),
                    Volume = Options.Volume,
                    IsLooping = Options.Loop
                };
                _player.SetSource(Options.Content.AsRandomAccessStream(), Options.MimeType);
            }
            [Obsolete("Only used in 0.10.0a105. Use Init(StreamPlayerOptions).")]
            public static StreamPlayer Create(Stream Content, bool Loop = false, float Volume = 1)
            {
                var Return = new StreamPlayer();
                Return.Init(Content, Loop, Volume);
                return Return;
            }
            [Obsolete("Only used in 0.10.0a105. Use Init(StreamPlayerOptions).")]
            protected void Init(Stream Content, bool Loop, float Volume)
            {
                _player = new MediaElement
                {
                    IsMuted = false,
                    Position = new TimeSpan(0, 0, 0),
                    Volume = Volume,
                    IsLooping = Loop
                };
                _player.SetSource(Content.AsRandomAccessStream(), GetMime(Content.ReadFully()));
            }
            public void Play()
            { _player.Play(); }
            public void Pause()
            { _player.Pause(); }
            public void Stop()
            { _player.Stop(); }
            public float Volume { get { return (float)_player.Volume; } set { _player.Volume = value; } }
            public event EventHandler Complete
            {
                add { _player.MediaEnded += (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
                remove { _player.MediaEnded -= (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
            }
            public bool Loop { get { return _player.IsLooping; } set { _player.IsLooping = value; } }
            [DllImport(@"urlmon.dll")]
            private extern static uint FindMimeFromData(uint pBC, [MarshalAs(UnmanagedType.LPStr)] string pwzUrl,
                                                        [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer, uint cbSize,
                                                        [MarshalAs(UnmanagedType.LPStr)] string pwzMimeProposed,
                                                        uint dwMimeFlags, out uint ppwzMimeOut, uint dwReserverd);
            public static string GetMime(byte[] buffer)
            {
                try
                {
                    uint mimetype;
                    FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                    IntPtr mimeTypePtr = new IntPtr(mimetype);
                    string mime = Marshal.PtrToStringUni(mimeTypePtr);
                    Marshal.FreeCoTaskMem(mimeTypePtr);
                    return mime;
                }
                catch (Exception)
                {
                    return "unknown/unknown";
                }
            }
#endif
            private StreamPlayer() : base() { }
        }
    }
}