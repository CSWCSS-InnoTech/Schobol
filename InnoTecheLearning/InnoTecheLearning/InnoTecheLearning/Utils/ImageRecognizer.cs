//From https://github.com/Synisse/eigenfaces
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;
//using ILNumerics;
//using ILNumerics.BuiltInFunctions;

namespace InnoTecheLearning
{
    partial class Utils
    {

        public class EigenfaceManager : IDisposable
        {

            #region Var
            //constants
            private const string TrainingIndicator = "1";

            //lists and arrays
            private readonly List<Bitmap> _trainingSet = new List<Bitmap>();
            private readonly List<byte[]> _vectorSet = new List<byte[]>();
            private readonly List<Bitmap> _eigenFaces = new List<Bitmap>();
            private readonly List<byte[]> _foundFaces = new List<byte[]>();
            private byte[,] _columnVectorMatrix;
            private double[,] _columnVectorIntMatrix;
            private byte[] _meanVector;
            private double[] _eigenValues = null;

            //Bitmaps
            public Bitmap TestBild;
            public Bitmap MeanFaceBitmap;
            public Bitmap InputPic;

            //ILNumerics
            private double[,] _covariance;
            private double[,] _a;
            private double[,] _u = new double[0, 0];
            private double[,] _s = new double[0, 0];
            private double[,] _v = new double[0, 0];

            //Vector + Value collection
            private readonly Collection<double[]> _eigenVectors = new Collection<double[]>();
            private readonly Collection<double[]> _eigenWeights = new Collection<double[]>();

            //boloean
            private Boolean _trainingSetLoaded = false;

            //dimensions
            private int _columnMatrixWidth = 0;
            private int _columnMatrixHeight = 0;

            //prop
            public int EigenfacesLoaded
            {
                get
                {
                    return _trainingSet.Count > 0 ? _trainingSet.Count : 0;
                }
            }

            #endregion

            /// <summary>
            /// Loads all images from given enumerable. 
            /// No need to dispose any images yourself - they will be disposed when this is disposed.
            /// </summary>
            /// <param name="path"></param>
            public void AddToTrainingSet(IEnumerable<Bitmap> b)
            {
                if (!_trainingSetLoaded)
                {
                    _trainingSet.AddRange(b);
                    _trainingSetLoaded = true;
                }
            }
            /// <summary>
            /// Loads all images from given Path, that contain a "1" in their Filename.
            /// </summary>
            /// <param name="path"></param>
            public void LoadTrainingSet(string path)
            {
                if (!_trainingSetLoaded)
                {
                    var dirInfo = new DirectoryInfo(path);
                    var imageList = new ArrayList();

                    foreach (var file in dirInfo.GetFiles())
                    {
                        if (file.Name.Contains(TrainingIndicator))
                        {
                            //open file
                            var image = Create.Bitmap(file.FullName);
                            _trainingSet.Add(image);
                        }
                    }

                    _trainingSetLoaded = true;
                }
            }

            /// <summary>
            /// Creates the Columnvector-Matrix.
            /// The Dimensions are Count-of-Images * (Image-Height*Image-Width).
            /// </summary>
            private void CreateColumnVectorMatrix()
            {
                var matrixHeightRef = _vectorSet[0];
                _columnMatrixHeight = matrixHeightRef.Length;
                _columnMatrixWidth = _vectorSet.Count;

                _columnVectorMatrix = new byte[_columnMatrixWidth, _columnMatrixHeight];

                for (int i = 0; i < _vectorSet.Count; i++)
                {
                    var vector = _vectorSet[i];
                    for (int j = 0; j < vector.Length; j++)
                    {
                        byte color = vector[j];
                        _columnVectorMatrix[i, j] = color;
                    }
                }
            }

            /// <summary>
            /// Creates a set of vectors of all previously loaded images.
            /// </summary>
            private void CreateVectorSet()
            {
                foreach (Bitmap bmp in _trainingSet)
                {
                    _vectorSet.Add(ImageManager.ConvertImageToVector(bmp));
                }
            }

            public void InitialiseEigenfaceManager()
            {
                CreateVectorSet();
                CreateColumnVectorMatrix();
                CalculateMeanOfColumMatrix();
                SubstractMeanVectorFromVectorFaceMatrix();
                ConvertColorToDoubleMatrix();
                CreateCovarianceMatrix();
                CreateSvdOnCorrelation();
                CalculateEigenValues();
                BuildAllEigenfaces();
            }

            /// <summary>
            /// Calculates the Meanvector of the ColumnVectorMatrix.
            /// Estimates the mean of every row.
            /// </summary>
            private void CalculateMeanOfColumMatrix()
            {
                _meanVector = new byte[_columnMatrixHeight];

                for (var i = 0; i < _columnMatrixHeight; i++)
                {
                    var sum = 0;
                    for (var j = 0; j < _columnMatrixWidth; j++)
                    {
                        byte color = _columnVectorMatrix[j, i];
                        sum += color;
                    }
                    var avg = 0;

                    avg = sum / _columnMatrixWidth;
                    _meanVector[i] = (byte)avg;
                }
            }

            /// <summary>
            /// Subtracts the Meanvector from the ColumnVectorMatrix.
            /// </summary>
            private void SubstractMeanVectorFromVectorFaceMatrix()
            {
                for (var j = 0; j < _columnMatrixWidth; j++)
                {
                    for (var i = 0; i < _columnMatrixHeight; i++)
                    {
                        var subtractedValue = _columnVectorMatrix[j, i] - _meanVector[i];
                        if (subtractedValue < 0) subtractedValue = 0;
                        _columnVectorMatrix[j, i] = (byte)subtractedValue;
                    }
                }

            }

            /// <summary>
            /// Converts the Color-based ColumnMatrix into an double array.
            /// </summary>
            private void ConvertColorToDoubleMatrix()
            {
                _columnVectorIntMatrix = new double[_columnMatrixWidth, _columnMatrixHeight];
                for (var i = 0; i < _columnMatrixWidth; i++)
                {
                    for (var j = 0; j < _columnMatrixHeight; j++)
                    {
                        _columnVectorIntMatrix[i, j] = _columnVectorMatrix[i, j];
                    }
                }
            }

            /// <summary>
            /// Creates the small Covariancematrix.
            /// </summary>
            private void CreateCovarianceMatrix()
            {
                _a = _columnVectorIntMatrix;
                _covariance = _a.TransposeRowsAndColumns().MatrixProduct(_a);
            }

            /// <summary>
            /// Calls the Singular Value Decomposition for the Covariancematrix.
            /// </summary>
            private void CreateSvdOnCorrelation()
            {
                var Intermediate = DenseMatrix.OfArray(_covariance).Svd(true);
                _s = Intermediate.W.AsArray();
                _v = Intermediate.VT.AsArray();
                _u = Intermediate.U.AsArray();
            }

            /// <summary>
            /// Calculates the eigenvalues and the eigenfaces.
            /// </summary>
            private void CalculateEigenValues()
            {
                //25 = magicnumber, just needed to initialize double array
                _eigenValues = new double[25];
                _s.GetDiagonal(ref _eigenValues);
                var eigenValuesPow = new double[_eigenValues.Length];

                for (var i = 0; i < eigenValuesPow.Length; i++)
                {
                    eigenValuesPow[i] = Math.Pow(_eigenValues[i], -0.5);
                }

                _eigenVectors.Clear();

                for (var i = 0; i < eigenValuesPow.Length; i++)
                {
                    var oneEigenVector = System.Linq.Enumerable.ToArray(_v.SliceColumn(i));
                    var oneFace = MatrixProduct(_a, oneEigenVector);
                    var oneFace2 = DenseVector.OfArray(oneFace).Multiply(eigenValuesPow[i]);

                    //25 = magicnumber, just needed to initialize double array
                    var eigenVectorArray = oneFace2.AsArray();

                    var distance = 0.0;
                    for (var j = 0; j < eigenVectorArray.Length; j++)
                    {
                        distance += Math.Pow(eigenVectorArray[j], 2.0);
                    }

                    distance = Math.Sqrt(distance);
                    for (var j = 0; j < eigenVectorArray.Length; j++)
                    {
                        eigenVectorArray[j] /= (float)distance;
                    }
                    this._eigenVectors.Add(eigenVectorArray);
                }

                this._eigenWeights.Clear();

                for (var i = 0; i < this._vectorSet.Count; i++)
                {
                    var existingWeight = this.GetEigenWeight(this._vectorSet[i], this._eigenVectors.Count);
                    this._eigenWeights.Add(existingWeight);
                }
            }

            /// <summary>
            /// Gets the Eigenvalues for a given vector of an image.
            /// </summary>
            /// <param name="pixels">Vector of imagepixels.</param>
            /// <param name="numOfVectors">Number of eigenfaces.</param>
            /// <returns>Returns the eigenvalues for the given imagevector.</returns>
            private double[] GetEigenWeight(byte[] pixels, int numOfVectors)
            {
                var result = new double[numOfVectors];
                var diff = new double[pixels.Length];
                for (var i = 0; i < diff.Length; i++)
                {

                    diff[i] = (double)pixels[i] - this._meanVector[i];
                }
                for (var j = 0; j < numOfVectors; j++)
                {
                    var W = 0d;
                    var vectorI = this._eigenVectors[j];
                    for (var i = 0; i < diff.Length; i++)
                    {
                        W += diff[i] * vectorI[i];
                    }
                    result[j] = W;
                }
                return result;
            }

            /// <summary>
            /// Builds all eigenfaces.
            /// </summary>
            private void BuildAllEigenfaces()
            {
                for (var i = 0; i < _eigenVectors.Count; i++)
                {
                    BuildEigenFace(_eigenVectors[i]);
                }
            }

            /// <summary>
            /// Builds an eigenface.
            /// </summary>
            /// <param name="eigenVectorArray">Eigenvector</param>
            private void BuildEigenFace(double[] eigenVectorArray)
            {
                var min = 0d;
                var max = 0d;
                for (var i = 0; i < eigenVectorArray.Length; i++)
                {
                    if (max < eigenVectorArray[i])
                    {
                        max = eigenVectorArray[i];
                    }
                    if (min > eigenVectorArray[i])
                    {
                        min = eigenVectorArray[i];
                    }
                }
                var eigenPixels = new byte[eigenVectorArray.Length];
                for (var i = 0; i < eigenPixels.Length; i++)
                {
                    eigenPixels[i] = (byte)(255f * ((eigenVectorArray[i] - min) / (max - min)));
                }
                var bitmap = ImageManager.ConvertVectorToImage(eigenPixels);
                _eigenFaces.Add(bitmap);

                //test
                TestBild = ImageManager.ConvertVectorToImage(eigenPixels);
                MeanFaceBitmap = ImageManager.ConvertVectorToImage(_meanVector);
            }

            /// <summary>
            /// Estimates the closest similar face for the inputface.
            /// It is possibly to estimate more than one resultimage.
            /// </summary>
            /// <param name="imageSource">Path of the inputimage</param>
            /// <returns>Return the most similar image, that was trained by the system.</returns>
            public Bitmap GetFaceForInput(string imageSource)
            {
                var image = Create.Bitmap(imageSource);
                InputPic = image;

                double[] newWeight = this.GetEigenWeight(ImageManager.ConvertImageToVector(image), _eigenVectors.Count);
                var sortedWeights = new Collection<double>();
                var sortedPictures = new Collection<byte[]>();
                _foundFaces.Clear();
                for (var i = 0; i < this._trainingSet.Count; i++)
                {
                    var distance = this.GetDistance(newWeight, this._eigenWeights[i]);

                    //0.05 = best distance
                    if (distance <= 0.05)
                    {
                        if (sortedWeights.Count == 0)
                        {
                            sortedWeights.Add(distance);
                            sortedPictures.Add(_vectorSet[i]);
                        }
                        else
                        {
                            for (var j = 0; j < sortedWeights.Count; j++)
                            {
                                if (distance < sortedWeights[j])
                                {
                                    sortedWeights.Insert(j, distance);
                                    sortedPictures.Insert(j, _vectorSet[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
                for (var i = 0; i < Math.Min(10, sortedWeights.Count); i++)
                {
                    this._foundFaces.Add(sortedPictures[i]);
                }

                return ImageManager.ConvertVectorToImage(_foundFaces[0]);
            }

            /// <summary>
            /// Estimates the difference of eigenvalues.
            /// </summary>
            /// <param name="newWeight">Eigenvalues of a new image</param>
            /// <param name="existingWeight">Eigenvalues of an already trained face</param>
            /// <returns>The distance.</returns>
            private double GetDistance(double[] newWeight, double[] existingWeight)
            {
                var result = 0.0;
                for (var i = 0; i < newWeight.Length; i++)
                {
                    result += Math.Pow((newWeight[i] - existingWeight[i]) / _eigenVectors[0].Length, 2.0);
                }
                result = Math.Sqrt(result);
                return result / Math.Sqrt(newWeight.Length);
            }

            /// <summary>
            /// Reconstructs the given face based on eigenvalues.
            /// </summary>
            /// <param name="image">Image considered to reconstruct.</param>
            /// <returns>Returns the reconstructed image.</returns>
            public Bitmap ReconstructFace(Bitmap image)
            {
                var weights = this.GetEigenWeight(ImageManager.ConvertImageToVector(image), _eigenVectors.Count);
                var reconstructedFace = this.CreateReconstructFace(weights);
                return ImageManager.ConvertVectorToImage(reconstructedFace);
            }

            /// <summary>
            /// Creates the reconstructed face based on its eigenvalues.
            /// </summary>
            /// <param name="weights">Eigenvalues of the face that should be reconstructed.</param>
            /// <returns>Return the vector of the reconstructed image.</returns>
            private byte[] CreateReconstructFace(double[] weights)
            {
                var result = new double[this._meanVector.Length];
                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = this._meanVector[i];
                }
                for (var j = 0; j < weights.Length; j++)
                {
                    var W = weights[j];
                    var vectorI = this._eigenVectors[j];

                    for (var i = 0; i < result.Length; i++)
                    {
                        result[i] += W * vectorI[i];
                    }
                }
                var resultB = new byte[this._meanVector.Length];
                for (var i = 0; i < resultB.Length; i++)
                {
                    resultB[i] = (byte)result[i];
                }
                return resultB;
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    foreach (var b in _trainingSet) b?.Dispose();
                    foreach (var b in _eigenFaces) b?.Dispose();
                    MeanFaceBitmap?.Dispose();
                    TestBild?.Dispose();
                    InputPic?.Dispose();
                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~EigenfaceManager()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            #endregion

            public static class ImageManager
            {
                private static int width;
                private static int height;
                public static byte[] ConvertImageToVector(Bitmap Image)
                {
                    byte[] vector = new byte[Image.Width * Image.Height];

                    height = Image.Height;
                    width = Image.Width;

                    int positionCounter = 0;

                    for (int i = 0; i < Image.Width; i++)
                    {
                        for (int j = 0; j < Image.Height; j++)
                        {
                            vector[positionCounter] = Image.GetPixel(i, j).R;
                            //var color = Image.GetPixel(i, j);
                            positionCounter++;
                        }
                    }

                    return vector;
                }

                public static Bitmap ConvertVectorToImage(byte[] pixels)
                {
                    Bitmap image = new Bitmap(width, height);

                    int positionCounter = 0;
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            positionCounter++;
                            SetPixel.Invoke(image, new object[] { i, j, FromArgb.Invoke(null,
                                new object[] { 255, pixels[positionCounter], pixels[positionCounter], pixels[positionCounter] }) });
                        }
                    }
                    return image;
                }

                static System.Reflection.MethodInfo SetPixel;
                static System.Reflection.MethodInfo FromArgb;
                static ImageManager()
                {
                    FromArgb = typeof(Bitmap).GetMethod(nameof(Bitmap.GetPixel)).ReturnType
                        .GetMethod("FromArgb", new[] { typeof(int), typeof(int), typeof(int), typeof(int) });
                    SetPixel = typeof(Bitmap).GetMethod(nameof(Bitmap.SetPixel));
                }

                //public BufferedImage ConvertVectorToImage(int[] ImageVector)
                //{

                //}
            }
        }

    }
}